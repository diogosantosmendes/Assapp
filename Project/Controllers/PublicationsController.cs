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
        /// <summary>
        /// 
        /// Apresenta a lista de publicações, mas como é a primeira chamada tem um view diferente
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                PublicationListViewModel list = new PublicationListViewModel
                {
                    // como a view não é paginada apresenta a página a -1
                    Index = -1,
                    // como apresenta apeenas 6 publicações  verifica se o total é superior a isso
                    HaveMore = db.Publication.Count() > 6 ? true : false,
                    // como é a primeira vista da listagem de publicações, não tem uma página inferior
                    HaveLess = false, 
                    // procura as ultimas 6 publicações aceites
                    List = db.Publication.Where(x => x.Accepted).OrderByDescending(p => p.CreatedIn).Take(6).ToList(), 
                    // procura as ultimas 4 publicações com imagem para apresentar no caroussel
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
        /// <summary>
        /// 
        /// Apresenta a lista de publicações paginadas
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public ActionResult Page(int i)
        {
            try
            {
                // se o indice da página for -1 redireciona para o controller correspondente à primeira vista da listagem de publicações
                if (i == -1) return RedirectToAction("Index");
                // a flag servirá de auxiliar, como ponteiro ao indice das publicações a retornar
                // como esta view é dedicada apenas a listagem de publicações, já serão retornadas 9 de cada vez
                int flag = i * 9 + 6;
                PublicationListViewModel list = new PublicationListViewModel
                {
                    Index = i,
                    // se a contagem for superior ao indice desta pagina na lista de publicações mais 9 então haverá mais publicações
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
        /// <summary>
        /// 
        /// Procura pela publicação pretendida e envia, caso não exista retorna à pagina inicial 
        /// 
        /// </summary>
        /// <param name="publicationID"></param>
        /// <returns></returns>
        public ActionResult Details(int? publicationID)
        {
            int index = publicationID ?? default(int);
            // se o id da publicação for inválido informa o bad request
            if (index >= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.Publication.Find(index);
            // se a publicação não existir ou, se for um publicação por aceitar e o utilizador não é um admin,
            // redireciona para a primeira vista da listagem de publicações
            if (model == null || (!User.IsInRole("admin") && !model.Accepted))
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }



        // GET: Publications/Unaccepted
        /// <summary>
        /// 
        /// Procura pelas publicações por aceitar e apresenta
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        public ActionResult Unaccepted()
        {
            return View(db.Publication.Where(x => !x.Accepted).OrderByDescending(p => p.CreatedIn).ToList());
        }

        // POST: Publications/Vote/
        /// <summary>
        /// 
        /// Executa o voto numa votação
        /// 
        /// </summary>
        /// <param name="optionID"></param>
        /// <param name="pollID"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Vote(int optionID, int pollID)
        {
            // Apenas os sócios, ou em caso de votações autorizadas também os associados, podem votar
            if (User.IsInRole("partner") || (db.Poll.Find(pollID).IsInclusive && User.IsInRole("associated")))
            {
                // verifica se existe a relações entre a opção de voto e a votação
                if (!db.Vote.Where(x => x.Option.PollFK.Equals(pollID)).Where(x => x.User.UserName.Equals(User.Identity.Name)).Any())
                {
                    // guarda o voto
                    Vote vote = new Vote { OptionFK = optionID, UserFK = User.Identity.GetUserId() };
                    db.Vote.Add(vote);
                    db.SaveChanges();
                    // incrementa o atributo da cotagem de votos na opção para estatisticas
                    Option option = db.Option.Find(optionID);
                    option.Count++;
                    db.Entry(option).State = EntityState.Modified;
                    db.SaveChanges();
                    // regista a ação do utilizador
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
        /// <summary>
        /// 
        /// Comenta numa publicação
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="publicationID"></param>
        /// <param name="publicationName"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "partner")]
        public JsonResult Comment(String text, int publicationID, String publicationName)
        {
            try
            {
                // guarda o comentário
                Reply reply = new Reply { Content = text, Hour = DateTime.Now, IsVisible = true, PublicationFK = publicationID, UserFK = User.Identity.GetUserId() };
                db.Reply.Add(reply);
                db.SaveChanges();
                // guarda a ação  do utilizador
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
        /// <summary>
        /// 
        /// Censura ou retira a censura de um comentário
        /// 
        /// </summary>
        /// <param name="replyID"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "collaborator")]
        public JsonResult Censor(int replyID)
        {
            try
            {
                // procura o comentário
                Reply r = db.Reply.Find(replyID);
                // se estiver visível coloca a invisivel e vice-versa
                r.IsVisible = r.IsVisible == true ? false : true;
                db.Entry(r).State = EntityState.Modified;
                db.SaveChanges();
                // regista a ação do utilizador
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
        /// <summary>
        /// 
        /// Encerra a votação
        /// 
        /// </summary>
        /// <param name="pollID"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "collaborator")]
        public JsonResult ClosePoll(int pollID)
        {
            try
            {
                Poll p = db.Poll.Find(pollID);
                // termina a votação
                p.IsFinished = true;
                // torna os resultados visiveis
                p.IsVisible = true; 
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                // regista a ação do utilizador
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
        /// <summary>
        /// 
        /// Aceita um publicação, de forma a ficar disponivel a todos os utilizadores
        /// 
        /// </summary>
        /// <param name="publicationID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// Cria uma nova publicação
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "collaborator")]
        public ActionResult Create([Bind(Include = "Name,Description,IsEvent,IsPoll,Day,Local,Matter,IsVisible,IsInclusive,LinkToForm,OptionName")] PublicationCreateViewModel form, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                // Cria a publicação
                Publication publication = new Publication { CreatedIn = DateTime.Now, Description = form.Description, Name = form.Name, Accepted = false, UserFK = User.Identity.GetUserId() };

                // verifica se no formulário foi solicitado um evento
                if (form.IsEvent)
                {
                    // cria o evento
                    Event e = new Event { Day = Convert.ToDateTime(form.Day), Local = form.Local };
                    db.Event.Add(e);
                    db.SaveChanges();
                    publication.Event = e;
                }
                // verifica se no formulário foi solicitada uma votação
                if (form.IsPoll)
                {
                    // cria a votação
                    Poll p = new Poll { Matter = form.Matter, IsFinished = false, IsInclusive = form.IsInclusive, IsVisible = form.IsVisible, LinkToForm = form.LinkToForm };
                    db.Poll.Add(p);
                    db.SaveChanges();
                    // Cria as opções de escolha da votação
                    foreach (String option in form.OptionName)
                    {
                        Option o = new Option { Name = option, Poll = p, Count = 0 };
                        db.Option.Add(o);
                        db.SaveChanges();
                    }
                    publication.Poll = p;
                }
                // verifica se foi enviadoo algum ficheiro
                if (file != null && file.ContentLength > 0)
                {
                    // guarda o ficheiro e guarda o nome do mesmo no atributo de imagem da publicação
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/fonts"), fileName);
                    file.SaveAs(path);
                    publication.Image = fileName;
                }

                db.Publication.Add(publication);
                db.SaveChanges();
                // regista a ação do utilizador
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
            // Cria o objeto da View para o formulário
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
        /// <summary>
        /// 
        /// Edita uma publicação
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,IsEvent,IsPoll,Day,Local,Matter,IsVisible,IsInclusive,LinkToForm,OptionName")] PublicationEditViewModel form, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                // procura na base de dados a publicação a alterar
                Publication publication = db.Publication.Find(form.ID);
                publication.Name = form.Name;
                publication.Description = form.Description;
                // verifica se a publicação originalmente tinha associado um evento
                if (publication.EventFK != null)
                {
                    // verifica se o formulário está editado como um evento
                    if (form.IsEvent)
                    {
                        // porcura o evento a alterar e altera
                        Event e = db.Event.Find(publication.EventFK);
                        e.Local = form.Local;
                        e.Day = Convert.ToDateTime(form.Day);
                        db.Entry(e).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    // senão coloca a null a referencia ao evento
                    else
                    {
                        publication.EventFK = null;
                    }
                }
                else
                {
                    // se originalmente não existia um evento associado, e foi editado com um evento então este é criado
                    if (form.IsEvent)
                    {
                        Event e = new Event { Local = form.Local, Day = Convert.ToDateTime(form.Day) };
                        db.Event.Add(e);
                        db.SaveChanges();
                        publication.Event = e;
                    }
                }
                // verifica se a publicação originalmente tinha associada uma votação
                if (publication.PollFK != null)
                {
                    // Verifica se o formulário foi definido com uma votação 
                    if (form.IsPoll)
                    {
                        // procura pela votação a editar
                        Poll p = db.Poll.Find(publication.PollFK);
                        p.LinkToForm = form.LinkToForm;
                        p.IsInclusive = form.IsInclusive;
                        p.IsVisible = form.IsVisible;
                        db.Entry(p).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    // senão coloca a referencia da votação na publicação a null 
                    else
                    {
                        publication.PollFK = null;
                    }
                }
                else
                {
                    // se a publicação originalmente não tinha associada uma votação, verific se o formu´lário já tem
                    if (form.IsPoll)
                    {
                        // cria uma nova publicação
                        Poll p = new Poll { LinkToForm = form.LinkToForm, IsInclusive = form.IsInclusive, IsVisible = form.IsVisible, Matter = form.Matter, IsFinished = false };
                        db.Poll.Add(p);
                        db.SaveChanges();
                        // crias as opções de escolha á votação
                        foreach (String option in form.OptionName)
                        {
                            Option o = new Option { Name = option, Poll = p, Count = 0 };
                            db.Option.Add(o);
                            db.SaveChanges();
                        }
                        publication.Poll = p;
                    }
                }

                // verifica se foi enviado um ficheiro
                if (file != null && file.ContentLength > 0)
                {
                    // guarda o ficheiro e altera o nome da imagem  na publicação
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/fonts"), fileName);
                    file.SaveAs(path);
                    publication.Image = fileName;
                }

                db.Entry(publication).State = EntityState.Modified;
                db.SaveChanges();
                // regista a ação do utilizador
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
