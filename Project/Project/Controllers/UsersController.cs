using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Users/Options
        public ActionResult Options()
        {
            var usersRole = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db))
                .Roles.Single(x => x.Name.Equals("partner"))
                .Users;

            List<User> users = new List<User>();

            foreach (var userRole in usersRole)
            {
                var user = db.Users.Where(x => x.Id.Equals(userRole.UserId)).FirstOrDefault();
                users.Add(user);
            }
            var ordered = users.OrderBy(x => x.Name).ToList();

            return View(ordered);
        }

        // POST: Users/Renew
        public async Task<ActionResult> Renew(int payPlan)
        {
            foreach (var userRole in new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db)).Roles.Single(x => x.Name.Equals("partner")).Users)
            {
                var user = UserManager.FindById(userRole.UserId);
                if (!UserManager.IsInRole(user.Id, "collaborator") && user.PayPlan == payPlan)
                {
                    var remove = await UserManager.RemoveFromRoleAsync(user.Id, "partner");
                    if (remove.Succeeded)
                    {
                        await UserManager.RemoveFromRoleAsync(user.Id, "associado");
                        Log("Remoção de permissões de sócio no âmbito de renovação do plano de pagamentos", user.Id);
                    }
                }
            }
            return RedirectToAction("Associated");
        }

        // POST: Users/Finish
        public async Task<ActionResult> Finish(String userID)
        {

            foreach (var userRole in new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db)).Roles.Single(x => x.Name.Equals("collaborator")).Users)
            {
                var remove = await UserManager.RemoveFromRoleAsync(userRole.UserId, "collaborator");
                if (remove.Succeeded)
                {
                    remove = await UserManager.RemoveFromRoleAsync(userRole.UserId, "admin");
                    if (remove.Succeeded)
                    {
                        Log("Remoção de permissões de administrador no âmbito de fim de mandato", userRole.UserId);
                    }
                    else
                    {
                        Log("Remoção de permissões de colaborador no âmbito de fim de mandato", userRole.UserId);
                    }
                }
            }

            var add = await UserManager.AddToRoleAsync(userID, "collaborator");
            if (add.Succeeded)
            {
                add = await UserManager.AddToRoleAsync(userID, "admin");
                if (add.Succeeded)
                {
                    Log("Foi promovido a administrador do sistema no âmbito de novo de mandato", userID);
                }
            }

            return RedirectToAction("Index", controllerName: "Home");
        }

        // GET: Users/Collaborator
        public ActionResult Collaborator()
        {
            var usersRole = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db))
                .Roles.Single(x => x.Name.Equals("collaborator"))
                .Users;

            List<User> users = new List<User>();

            foreach (var userRole in usersRole)
            {
                users.Add(UserManager.FindById(userRole.UserId));
            }

            var ordered = users.OrderBy(x => x.Name).ToList();

            return View(ordered);
        }

        // GET: Users/Partner
        public ActionResult Partner()
        {
            var usersRole = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db))
                .Roles.Single(x => x.Name.Equals("partner"))
                .Users;

            List<User> users = new List<User>();

            foreach (var userRole in usersRole)
            {
                if (!UserManager.IsInRole(userRole.UserId, "collaborator"))
                {
                    users.Add(UserManager.FindById(userRole.UserId));
                }
            }
            var ordered = users.OrderBy(x => x.Name).ToList();

            return View(ordered);
        }

        // GET: Users/Associated
        public ActionResult Associated()
        {
            var usersRole = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db))
                .Roles.Single(x => x.Name.Equals("associated"))
                .Users;

            List<User> users = new List<User>();

            foreach (var userRole in usersRole)
            {
                if (!UserManager.IsInRole(userRole.UserId, "partner"))
                {
                    users.Add(UserManager.FindById(userRole.UserId));
                }
            }
            var ordered = users.OrderBy(x => x.Name).ToList();

            return View(ordered);
        }

        // GET: Users/Pending
        public ActionResult Pending()
        {
            var usersRole = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db))
                .Roles.Single(x => x.Name.Equals("pending"))
                .Users;

            List<User> users = new List<User>();

            foreach (var userRole in usersRole)
            {
                users.Add(UserManager.FindById(userRole.UserId));
            }

            return View(users);
        }

        // POST: Users/Users/PromoteToStaff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PromoteToStaff(String userID, String role)
        {
            var add = await UserManager.AddToRoleAsync(userID, "collaborator");
            if (add.Succeeded)
            {
                if (role == "admin")
                {
                    await UserManager.AddToRoleAsync(userID, role);
                    Log(String.Format("Foi promovido a administrador do sistema pelo atual administrador {0}", UserManager.FindById(User.Identity.GetUserId()).Name), userID);
                }
                else
                    Log(String.Format("Foi promovido a colaborador no sistema pelo atual administrador {0}", UserManager.FindById(User.Identity.GetUserId()).Name), userID);
            }
            return RedirectToAction("Partner");
        }

        // POST: Users/Users/Demote
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Demote(String userID)
        {
            if (!UserManager.GetRoles(userID).Contains("admin"))
            {
                var remove = await UserManager.RemoveFromRoleAsync(userID, "collaborator");
                if (remove.Succeeded)
                {
                    Log(String.Format("Foi destituido de colaborador do sistema pelo atual administrador {0}", UserManager.FindById(User.Identity.GetUserId()).Name), userID);
                }
            }

            return RedirectToAction("Collaborator");
        }

        // POST: Users/Users/PromoteToPartner
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PromoteToPartner(int payPlan, int partner, String userID)
        {
            var user = UserManager.FindById(userID);
            user.DuePayday = DateTime.Now;
            user.PayPlan = payPlan;
            if (user.Partner == null)
            {
                user.Partner = partner == 0 ? db.Users.Max(x => x.Partner) + 1 : partner;
            }
            var update = await UserManager.UpdateAsync(user);
            if (update.Succeeded)
            {
                var add = await UserManager.AddToRoleAsync(userID, "partner");
                if (add.Succeeded)
                {
                    Log(String.Format("Foi promovido a sócio pelo atual colaborador {0}", UserManager.FindById(User.Identity.GetUserId()).Name), userID);
                }
            }
            return RedirectToAction("Associated");
        }

        // POST: Users/PromoteToAssociated
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PromoteToAssociated(String userID)
        {
            var add = await UserManager.AddToRoleAsync(userID, "associated");
            if (add.Succeeded)
            {
                var remove = await UserManager.RemoveFromRoleAsync(userID, "pending");
                if (remove.Succeeded)
                {
                    Log(String.Format("Foi aceite como associado pelo atual colaborador {0}", UserManager.FindById(User.Identity.GetUserId()).Name), userID);
                }
            }
            return RedirectToAction("Pending");
        }

        // GET: Users/Log
        [HttpGet]
        public JsonResult Log(string userID, int page)
        {
            try
            {
                var logs = db.Log.Where(x => x.UserFK == userID).ToList();
                var toSend = db.Log.Where(x => x.UserFK == userID)
                    .OrderByDescending(x => x.Hour)
                    .Skip(page * 20)
                    .Take(20)
                    .Select(x=>new { Hour=x.Hour.ToString(), x.Description });
                var hasmore = logs.Count() > (page * 20 + 20) ? true : false;
                if (toSend.Count() > 0)
                {
                    return Json(new { result = true, logs = toSend, page = page, hasmore = hasmore, user = userID }, JsonRequestBehavior.AllowGet);
                }
                if (page == 0)
                {
                    return Json(new { result = false, msg = "Não foram encontradas ações do utilizador." }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false, msg = "A página excedeu o limite de ações do utilizador." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Json(new { result = false, msg = "Ocorreu um erro, não foi possível encontrar ações do utilizador. Por favor tente mais tarde." }, JsonRequestBehavior.AllowGet);
            }

        }

        private void Log(String content, String userID)
        {
            Log l = new Log { UserFK = userID, Hour = DateTime.Now, Description = content };
            db.Log.Add(l);
            db.SaveChanges();
        }


    }
}
