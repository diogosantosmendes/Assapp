using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            XDocument xml = XDocument.Load(Server.MapPath("/App_Data/home.xml"));
            var root = xml.Element("home");
            var model = new HomeIndexViewModel
            {
                Name = root.Attribute("name").Value,
                Regulation = root.Attribute("regulation").Value,
                AboutUs = root.Element("aboutus").Value,
                Mission= root.Element("mission").Value,
                WhoWeAre= root.Element("whoweare").Value,
                Location_Lat = root.Element("location").Attribute("lat").Value,
                Location_Lon = root.Element("location").Attribute("lon").Value,
                TeamPhoto= root.Element("whoweare").Attribute("photo").Value,
                Image = root.Attribute("image").Value,
            };
            return View(model);
        }
    }
}