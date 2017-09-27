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
using System.Net;
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
        /// <summary>
        /// 
        /// Apresenta a vista das opções onde o administrador pode terminar o mandato
        /// ou renovar um plano de pagamento de quotas
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "collaborator")]
        public ActionResult Options()
        {
            // lista todos os sócios
            var usersRole = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db))
                .Roles.Single(x => x.Name.Equals("partner"))
                .Users;
            // Cria uma lista com os dados dos utilizadores de role "partner"
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
        /// <summary>
        /// 
        /// Renova um plano de pagamento de quotas, 
        /// o que implica na retirada de permissões de sócios aos utilizadores sócios
        /// 
        /// </summary>
        /// <param name="payPlan"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Renew(int payPlan)
        {
            // procura e percorre todos os utilizadores de role "partner"
            foreach (var userRole in new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db)).Roles.Single(x => x.Name.Equals("partner")).Users)
            {
                var user = UserManager.FindById(userRole.UserId);
                // se o utilizador não for colaborador e o plano de pagamento de quotas for o definido é-lhe retirado as permissões de sócio "partner"
                if (!UserManager.IsInRole(user.Id, "collaborator") && user.PayPlan == payPlan)
                {
                    var remove = await UserManager.RemoveFromRoleAsync(user.Id, "partner");
                    if (remove.Succeeded)
                    {
                        // Regista a ação no utilizador
                        Log("Remoção de permissões de sócio no âmbito de renovação do plano de pagamentos", user.Id);
                    }
                }
            }
            return RedirectToAction("Associated");
        }

        // POST: Users/Finish
        /// <summary>
        /// 
        /// Termina o mandato, elegendo um sócio para o posto de administrador
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Finish(String userID)
        {
            // retira as permissões de colaboradores e administradores a todos os elementos do orgão social
            foreach (var userRole in new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db)).Roles.Single(x => x.Name.Equals("collaborator")).Users)
            {
                var remove = await UserManager.RemoveFromRoleAsync(userRole.UserId, "collaborator");
                if (remove.Succeeded)
                {
                    remove = await UserManager.RemoveFromRoleAsync(userRole.UserId, "admin");
                    // Regista a ação no utilizador em causa
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
            // Atribui as permissões de colaborador e administrador ao sócio eleito
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
        /// <summary>
        /// 
        /// Apresenta a lista de colaboradores
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "collaborator")]
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
        /// <summary>
        /// 
        /// Apresenta a lista de todos os sócios que não sejam colaboradores
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "collaborator")]
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
        /// <summary>
        /// 
        /// Apresenta a lista de todos os associados que não sejam sócios
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "collaborator")]
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
        /// <summary>
        /// 
        /// Apresenta a lista de todos os utilizadores que se registaram no sistema 
        /// e aguardam para serem aceites como associados
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "collaborator")]
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
        /// <summary>
        /// 
        /// Promove um sócio a membro do órgão social
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> PromoteToStaff(String userID, String role)
        {
            // atribui as permissões de colaborador
            var add = await UserManager.AddToRoleAsync(userID, "collaborator");
            if (add.Succeeded)
            {
                // atribui as permissões de admin, caso o utilizador assim o defina, e regista a ação ao utilizador
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
        /// <summary>
        /// 
        /// Despromove um colaborador a sócio
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Demote(String userID)
        {
            // um admin nunca pode ser despromovido a menos que seja o fim de mandato
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
        /// <summary>
        /// 
        /// Promove um associado a sócio
        /// 
        /// </summary>
        /// <param name="payPlan"></param>
        /// <param name="partner"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "collaborator")]
        public async Task<ActionResult> PromoteToPartner(int payPlan, int partner, String userID)
        {
            try
            {
                var user = UserManager.FindById(userID);
                // guarda o plano de pagamento e a data de pagamento da quota
                user.DuePayday = DateTime.Now;
                user.PayPlan = payPlan;
                // se não for definido um numero de sócio será atribuido o numero imediatamente a seguir ao ultimo
                if (user.Partner == null)
                {
                    user.Partner = partner == 0 ? db.Users.Max(x => x.Partner) + 1 : partner;
                }
                var update = await UserManager.UpdateAsync(user);
                if (update.Succeeded)
                {
                    // Adiciona ao role de "partner"
                    var add = await UserManager.AddToRoleAsync(userID, "partner");
                    if (add.Succeeded)
                    {
                        // regista a ação no utilizador
                        Log(String.Format("Foi promovido a sócio pelo atual colaborador {0}", UserManager.FindById(User.Identity.GetUserId()).Name), userID);
                    }
                }
                return RedirectToAction("Associated");
            }catch(Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
        }

        // POST: Users/PromoteToAssociated
        /// <summary>
        /// 
        /// Promove um utilizador pendente a associado
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "collaborator")]
        public async Task<ActionResult> PromoteToAssociated(String userID)
        {
            // adiciona o role de associado
            var add = await UserManager.AddToRoleAsync(userID, "associated");
            if (add.Succeeded)
            {
                // retira do role de pendente
                var remove = await UserManager.RemoveFromRoleAsync(userID, "pending");
                if (remove.Succeeded)
                {
                    // regista a ação no utilizador
                    Log(String.Format("Foi aceite como associado pelo atual colaborador {0}", UserManager.FindById(User.Identity.GetUserId()).Name), userID);
                }
            }
            return RedirectToAction("Pending");
        }

        // GET: Users/Log
        /// <summary>
        /// 
        /// Apresenta todas as ações do utilizador no sistema, ou que tenham afetado o utilizador
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "collaborator")]
        public JsonResult Log(string userID, int page)
        {
            try
            {
                // procura os 20 registos (se existirem) que correspondem à pagina requirida
                var logs = db.Log.Where(x => x.UserFK == userID).ToList();
                var toSend = db.Log.Where(x => x.UserFK == userID)
                    .OrderByDescending(x => x.Hour)
                    .Skip(page * 20)
                    .Take(20)
                    .Select(x=>new { Hour=x.Hour.ToString(), x.Description });
                var hasmore = Math.Floor((decimal) logs.Count()/20) >  page ? true : false;
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
