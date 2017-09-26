using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Diagnostics;
using System.Web.UI.WebControls;

namespace Project.Controllers
{
    public class PublicationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Publications
        public ActionResult Index()
        {
            try
            {
                PublicationListViewModel list = new PublicationListViewModel
                {
                    Index = -1,
                    HaveMore = db.Publication.Count() > 6 ? true : false,
                    HaveLess = false,
                    List = db.Publication.Where(x => x.Accepted).OrderByDescending(p => p.CreatedIn).Take(6).ToList(),
                    Header = db.Publication.Where(x => x.Accepted && x.Image != null).OrderByDescending(p => p.CreatedIn).Take(4).ToList()
                };
                return View(list);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Publications
        public ActionResult Page(int i)
        {
            try
            {
                if (i == -1) return RedirectToAction("Index");
                int flag = i * 9 + 6;
                PublicationListViewModel list = new PublicationListViewModel
                {
                    Index = i,
                    HaveMore = db.Publication.Count() > (flag + 9) ? true : false,
                    HaveLess = true,
                    List = db.Publication.Where(x => x.Accepted).OrderByDescending(p => p.CreatedIn).Skip(flag).Take(flag + 9).ToList(),
                    Header = null
                };
                return View("Index", list);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Publications/Profile
        public ActionResult Details(int? publicationID)
        {
            int index = publicationID ?? default(int);
            return View(db.Publication.Find(index));
        }



        // GET: Publications/Unaccepted
        [Authorize(Roles = "admin")]
        public ActionResult Unaccepted()
        {
            return View(db.Publication.Where(x => !x.Accepted).OrderByDescending(p => p.CreatedIn).ToList());
        }

        // POST: Publications/Vote/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Vote(int optionID, int pollID)
        {
            if (User.IsInRole("partner") || (db.Poll.Find(pollID).IsInclusive && User.IsInRole("associated")))
            {
                if (!db.Vote.Where(x => x.Option.PollFK.Equals(pollID)).Where(x => x.User.UserName.Equals(User.Identity.Name)).Any())
                {
                    Vote vote = new Vote { OptionFK = optionID, UserFK = User.Identity.GetUserId() };
                    db.Vote.Add(vote);
                    db.SaveChanges();
                    Option option = db.Option.Find(optionID);
                    option.Count++;
                    db.Entry(option).State = EntityState.Modified;
                    db.SaveChanges();
                    Log(String.Format("Votou no questionário: {0}", db.Poll.Find(pollID).Matter));
                    return Json(new { result = true, msg = "Voto inserido com sucesso." });
                }
                else
                {
                    return Json(new { result = false, msg = "Já tinha votado anteriormente." });
                }
            }
            else
            {
                return Json(new { result = false, msg = "Não autorizado." });
            }

        }

        // POST: Publications/Comment/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "partner")]
        public JsonResult Comment(String text, int publicationID, String publicationName)
        {
            try
            {
                Reply reply = new Reply { Content = text, Hour = DateTime.Now, IsVisible = true, PublicationFK = publicationID, UserFK = User.Identity.GetUserId() };
                db.Reply.Add(reply);
                db.SaveChanges();
                Log(String.Format("Comentou ( {0} ) na publicação: {1}", text, publicationName));
                return Json(new { result = true, publication = publicationID, hour = reply.Hour.ToString("dd MMM yyyy - hh:mm"), user = db.Users.Find(User.Identity.GetUserId()).Name.ToString() });
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return Json(new { result = false, msg = "Ocorreu um erro, não foi possível adicionar o seu comentário. Por favor tente mais tarde." });
            }
        }

        // POST: Publications/Censor/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "collaborator")]
        public JsonResult Censor(int replyID)
        {
            try
            {
                Reply r = db.Reply.Find(replyID);
                r.IsVisible = r.IsVisible == true ? false : true;
                db.Entry(r).State = EntityState.Modified;
                db.SaveChanges();
                Log(String.Format("Censurou o comentário ( {0} ) de {1} na publicação: {2}", r.Content, r.User.Name, r.Publication.Name));
                return Json(new { result = true, msg = "A censura foi realizada com sucesso. Atualize a página se pretender retroceder." });
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return Json(new { result = false, msg = "Ocorreu um erro, não foi possível censurar o comentário. Por favor tente mais tarde." });
            }
        }

        // POST: Publications/Censor/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "collaborator")]
        public JsonResult ClosePoll(int pollID)
        {
            try
            {
                Poll p = db.Poll.Find(pollID);
                p.IsFinished = true;
                p.IsVisible = true;
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                Log(String.Format("Encerrou a votação: {0}", p.Matter));
                return Json(new { result = true, msg = "A votação foi finalizada com sucesso. Atualize a página para consultar os resultados." });
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return Json(new { result = false, msg = "Ocorreu um erro, não foi possível encerrar a votação. Por favor tente mais tarde." });
            }
        }

        // POST: Publications/Accept/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Accept(int publicationID)
        {
            Publication p = db.Publication.Find(publicationID);
            p.Accepted = true;
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();
            Log(String.Format("Permitiu a publicação: {0}", p.Name));
            return RedirectToAction("Unaccepted");
        }

        // GET: Publications/Create
        [Authorize(Roles = "collaborator")]
        public ActionResult Create()
        {
            return View();
        }


        // POST: Publications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "collaborator")]
        public ActionResult Create([Bind(Include = "Name,Description,IsEvent,IsPoll,Day,Local,Matter,IsVisible,IsInclusive,LinkToForm,OptionName")] PublicationCreateViewModel form, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                Publication publication = new Publication { CreatedIn = DateTime.Now, Description = form.Description, Name = form.Name, Accepted = false, UserFK = User.Identity.GetUserId() };

                if (form.IsEvent)
                {
                    Event e = new Event { Day = Convert.ToDateTime(form.Day), Local = form.Local };
                    db.Event.Add(e);
                    db.SaveChanges();
                    publication.Event = e;
                }

                if (form.IsPoll)
                {
                    Poll p = new Poll { Matter = form.Matter, IsFinished = false, IsInclusive = form.IsInclusive, IsVisible = form.IsVisible, LinkToForm = form.LinkToForm };
                    db.Poll.Add(p);
                    db.SaveChanges();
                    foreach (String option in form.OptionName)
                    {
                        Option o = new Option { Name = option, Poll = p, Count = 0 };
                        db.Option.Add(o);
                        db.SaveChanges();
                    }
                    publication.Poll = p;
                }

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/fonts"), fileName);
                    file.SaveAs(path);
                    publication.Image = fileName;
                }

                db.Publication.Add(publication);
                db.SaveChanges();
                Log(String.Format("Criou uma nova publicação: {0}", publication.Name));
                return RedirectToAction("Index");
            }
            return View("Create");
        }

        // GET: Publications/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var publication = db.Publication.Find(id);

            if (publication == null)
            {
                return HttpNotFound();
            }

            PublicationEditViewModel viewModel = new PublicationEditViewModel
            {
                ID = publication.ID,
                Name = publication.Name,
                Description = publication.Description,
                Image = publication.Image,
                IsEvent = publication.EventFK != null ? true : false,
                IsPoll = publication.PollFK != null ? true : false,
                Day = publication.EventFK != null ? publication.Event.Day : (DateTime?)null,
                Local = publication.EventFK != null ? publication.Event.Local : null,
                Matter = publication.PollFK != null ? publication.Poll.Matter : null,
                IsVisible = publication.PollFK != null ? publication.Poll.IsVisible : false,
                IsInclusive = publication.PollFK != null ? publication.Poll.IsInclusive : false,
                LinkToForm = publication.PollFK != null ? publication.Poll.LinkToForm : null,
                OptionName = publication.PollFK != null ? publication.Poll.Options.Select(z => z.Name).ToArray() : new List<String>().ToArray()
            };

            return View(viewModel);
        }

        // POST: Publications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,IsEvent,IsPoll,Day,Local,Matter,IsVisible,IsInclusive,LinkToForm,OptionName")] PublicationEditViewModel form, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                Publication publication = db.Publication.Find(form.ID);
                publication.Name = form.Name;
                publication.Description = form.Description;

                if (publication.EventFK != null)
                {
                    if (form.IsEvent)
                    {
                        Event e = db.Event.Find(publication.EventFK);
                        e.Local = form.Local;
                        e.Day = Convert.ToDateTime(form.Day);
                        db.Entry(e).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        publication.EventFK = null;
                    }
                }
                else
                {
                    if (form.IsEvent)
                    {
                        Event e = new Event { Local = form.Local, Day = Convert.ToDateTime(form.Day) };
                        db.Event.Add(e);
                        db.SaveChanges();
                        publication.Event = e;
                    }
                }

                if (publication.PollFK != null)
                {
                    if (form.IsPoll)
                    {
                        Poll p = db.Poll.Find(publication.PollFK);
                        p.LinkToForm = form.LinkToForm;
                        p.IsInclusive = form.IsInclusive;
                        p.IsVisible = form.IsVisible;
                        db.Entry(p).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        publication.PollFK = null;
                    }
                }
                else
                {
                    if (form.IsPoll)
                    {
                        Poll p = new Poll { LinkToForm = form.LinkToForm, IsInclusive = form.IsInclusive, IsVisible = form.IsVisible, Matter = form.Matter, IsFinished = false };
                        db.Poll.Add(p);
                        db.SaveChanges();

                        foreach (String option in form.OptionName)
                        {
                            Option o = new Option { Name = option, Poll = p, Count = 0 };
                            db.Option.Add(o);
                            db.SaveChanges();
                        }
                        publication.Poll = p;
                    }
                }


                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/fonts"), fileName);
                    file.SaveAs(path);
                    publication.Image = fileName;
                }

                db.Entry(publication).State = EntityState.Modified;
                db.SaveChanges();

                Log(String.Format("Editou a publicação: {0}", publication.Name));
                return RedirectToAction("Index");
            }
            return View("Edit");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Log(String content)
        {
            Log l = new Log { UserFK = User.Identity.GetUserId(), Hour = DateTime.Now, Description = content };
            db.Log.Add(l);
            db.SaveChanges();
        }
    }
}
