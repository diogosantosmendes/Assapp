using Project.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home/Index
        public ActionResult Index()
        {
            XDocument xml = XDocument.Load(Server.MapPath("/App_Data/home.xml"));
            var root = xml.Element("HomeIndexViewModel");
            var model = new HomeIndexViewModel
            {
                Name = root.Element("Name").Value,
                Regulation = root.Element("Regulation").Value,
                AboutUs = root.Element("AboutUs").Value,
                Mission = root.Element("Mission").Value,
                WhoWeAre = root.Element("WhoWeAre").Value,
                Location_Lat = root.Element("Location_Lat").Value,
                Location_Lon = root.Element("Location_Lon").Value,
                TeamPhoto = root.Element("TeamPhoto").Value,
                Image = root.Element("Image").Value,
            };
            return View(model);
        }

        // GET: Home/Edit
        [Authorize(Roles = "admin")]
        public ActionResult Edit()
        {
            XDocument xml = XDocument.Load(Server.MapPath("/App_Data/home.xml"));
            var root = xml.Element("HomeIndexViewModel");
            var model = new HomeIndexViewModel
            {
                Name = root.Element("Name").Value,
                Regulation = root.Element("Regulation").Value,
                AboutUs = root.Element("AboutUs").Value,
                Mission = root.Element("Mission").Value,
                WhoWeAre = root.Element("WhoWeAre").Value,
                Location_Lat = root.Element("Location_Lat").Value,
                Location_Lon = root.Element("Location_Lon").Value,
                TeamPhoto = root.Element("TeamPhoto").Value,
                Image = root.Element("Image").Value,
            };
            return View(model);
        }

        // POST: Publications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "Name,AboutUs,Mission,WhoWeAre,Location_Lat,Location_Lon,Image,Regulation,TeamPhoto")] HomeIndexViewModel form, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3)
        {
            if (ModelState.IsValid)
            {
                if (file1 != null)
                {
                    if (file1.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file1.FileName);
                        var path = Path.Combine(Server.MapPath("~/fonts"), fileName);
                        file1.SaveAs(path);
                        form.Image = fileName;
                    }
                }
                if (file2 != null)
                {
                    if (file2.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file2.FileName);
                        var path = Path.Combine(Server.MapPath("~/fonts"), fileName);
                        file2.SaveAs(path);
                        form.TeamPhoto = fileName;
                    }
                }
                if (file3 != null)
                {
                    if (file3.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file3.FileName);
                        var path = Path.Combine(Server.MapPath("~/fonts"), fileName);
                        file3.SaveAs(path);
                        form.Regulation = fileName;
                    }
                }

                XmlSerializer writer = new XmlSerializer(typeof(HomeIndexViewModel));
                FileStream filestream = System.IO.File.Create(Server.MapPath("/App_Data/home.xml"));

                writer.Serialize(filestream, form);
                filestream.Close();
                return RedirectToAction("Index");
            }
            return View("Edit");
        }
    }
}