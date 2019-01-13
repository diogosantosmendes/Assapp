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
        /// <summary>
        /// 
        /// Carrega do ficheiro XML os dados da homepage e apresenta
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // faz o parsing do documento xml 
            XDocument xml = XDocument.Load(Server.MapPath("/App_Data/home.xml"));
            var root = xml.Element("HomeIndexViewModel");
            // cria o objeto a apresentar na View
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
        /// <summary>
        /// 
        /// Carrega do ficheiro XML os dados da homepage e apresenta na View para editar
        /// Apenas o admin tem permissão a editar a Homepage
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        public ActionResult Edit()
        {
            // faz o parsing do ficheiro
            XDocument xml = XDocument.Load(Server.MapPath("/App_Data/home.xml"));
            var root = xml.Element("HomeIndexViewModel");
            // cria o objeto a apresentar na view
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
        /// <summary>
        /// 
        /// Edita a home page, para isso, recebe por parametro 
        /// o objeto que define a view da homepage e 3 objetos 
        /// que correspondem à imagem de topo, a imagem da equipa
        /// e o ficheiro do regulamento
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <param name="file3"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "Name,AboutUs,Mission,WhoWeAre,Location_Lat,Location_Lon,Image,Regulation,TeamPhoto")] HomeIndexViewModel form, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3)
        {
            if (ModelState.IsValid)
            {
                // se o file1 existir, significa que a imagem de topo foi alterada
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
                // se o file2 existir, significa que a imagem da equipa foi alterada
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
                // se o file3 existir, significa que o regulamento foi alterado
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
                // inicia os objetos de serialização e escrita em ficheiros
                XmlSerializer writer = new XmlSerializer(typeof(HomeIndexViewModel));
                FileStream filestream = System.IO.File.Create(Server.MapPath("/App_Data/home.xml"));
                // reescreve o ficheiro xml com os dados da homepage
                writer.Serialize(filestream, form);
                filestream.Close();
                return RedirectToAction("Index");
            }
            return View("Edit");
        }
    }
}