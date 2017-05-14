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

namespace Project.Controllers
{
    public class PublicationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Publications
        public ActionResult Index()
        {
            var publications = db.Publication.Include(p => p.Event).Include(p => p.Poll).Include(p => p.User).OrderByDescending(p => p.CreatedIn);
            List<PublicationFeedViewModel> model = new List<PublicationFeedViewModel>();
            foreach(Publication pub in publications)
            {
                if (pub.Accepted)
                {
                    PublicationFeedViewModel newPub = new PublicationFeedViewModel {
                        ID =pub.ID,
                        Name = pub.Name,
                        Image = (pub.Image!=null)?"~/fonts/" + pub.Image:null,
                        Summary = pub.Summary,
                        CreatedIn = pub.CreatedIn
                    };
                    if (pub.PollFK != null)
                    {
                        if (pub.EventFK != null)
                        {
                            newPub.IsEvent = true;
                            newPub.IsPoll = true;
                        }
                        else newPub.IsPoll = true;
                    }
                    else 
                        if (pub.EventFK != null)
                            newPub.IsEvent = true;
                    model.Add(newPub);
                }
            }
            return View(model);
        }

        // GET: Publications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publication publication = db.Publication.Where(x => x.ID == id).Include(p => p.Event).Include(p => p.Poll).FirstOrDefault();
            if (publication == null)
            {
                return HttpNotFound();
            }
            publication.Image = (publication.Image != null) ? "~/fonts/" + publication.Image : "~/fonts/semImg.bmp";
            if(publication.Poll!=null)
                publication.Poll.Choices = db.Choice.Where(x => x.PollFK == publication.PollFK).Include(x=>x.Option).ToList();
            return View(publication);
        }

        // POST: Publications/Details/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Vote(int OptionID, int PollID, int PublicationID)
        {
            try
            {
                Vote vote = new Vote { OptionFK=OptionID, PollFK=PollID, UserFK= User.Identity.GetUserId() };
                db.Vote.Add(vote);
                db.SaveChanges();
                Choice choice = db.Choice.Where(x => x.OptionFK == OptionID).Where(x => x.PollFK == PollID).FirstOrDefault();
                choice.Count++;
                db.Entry(choice).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Inf"] = "Your vote was successfully inserted.";

            }
            catch(Exception)
            {
                TempData["Err"] = "You have already voted on this poll";
            }
            
            return RedirectToAction("Details", new { id=PublicationID});
        }

        // GET: Publications/Create
        public ActionResult Create()
        {
            ViewBag.EventFK = new SelectList(db.Event, "ID", "Local");
            ViewBag.PollFK = new SelectList(db.Poll, "ID", "Matter");
            ViewBag.UserFK = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: Publications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Image,Description,Summary,Accepted,LinkToForm,UserFK,PollFK,EventFK")] Publication publication)
        {
            if (ModelState.IsValid)
            {
                db.Publication.Add(publication);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventFK = new SelectList(db.Event, "ID", "Local", publication.EventFK);
            ViewBag.PollFK = new SelectList(db.Poll, "ID", "Matter", publication.PollFK);
            ViewBag.UserFK = new SelectList(db.Users, "Id", "Name", publication.UserFK);
            return View(publication);
        }

        // GET: Publications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publication publication = db.Publication.Find(id);
            if (publication == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventFK = new SelectList(db.Event, "ID", "Local", publication.EventFK);
            ViewBag.PollFK = new SelectList(db.Poll, "ID", "Matter", publication.PollFK);
            ViewBag.UserFK = new SelectList(db.Users, "Id", "Name", publication.UserFK);
            return View(publication);
        }

        // POST: Publications/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Image,Description,Summary,Accepted,LinkToForm,UserFK,PollFK,EventFK")] Publication publication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(publication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventFK = new SelectList(db.Event, "ID", "Local", publication.EventFK);
            ViewBag.PollFK = new SelectList(db.Poll, "ID", "Matter", publication.PollFK);
            ViewBag.UserFK = new SelectList(db.Users, "Id", "Name", publication.UserFK);
            return View(publication);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
