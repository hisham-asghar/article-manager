using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using Website.Models;
using System.Web.Mvc;
using LayerDb.Models;
using Microsoft.AspNet.Identity;

namespace Website.Controllers.Articles
{
    [Authorize]
    public class ArticlesController : Controller
    {
        private DbModel db = new DbModel();

        // GET: Articles
        public ActionResult Index()
        {
            var myUserId = User.Identity.GetUserId();
            var myRoles = LayerBao.UserBao.GetRoles(myUserId);
            var onlyMyArticles = myRoles.Count() == 0;
            var articles = db.Articles.Include(a => a.AspNetUser).Include(a => a.AspNetUser1);
            if (onlyMyArticles)
                articles = articles.Where(a => a.CreatedBy == myUserId);
            if (myRoles.Contains(Models.Constants.UserRoles.Reviewer))
            {
                articles = articles.Where(a => a.CreatedBy == myUserId || a.IsReadyToPublish || (a.IsReadyToReview && a.ArticleReviews.Count() == 0));
            }
            else if (myRoles.Contains(Models.Constants.UserRoles.Reader))
            {
                articles = articles.Where(a => a.CreatedBy == myUserId || a.IsReadyToPublish);
            }
            return View(articles.ToList());
        }
        
        // GET: Articles
        public ActionResult MyArticles()
        {
            var myUserId = User.Identity.GetUserId();
            var articles = db.Articles.Where(a => a.CreatedBy == myUserId).Include(a => a.AspNetUser).Include(a => a.AspNetUser1);
            return View("Index", articles.ToList());
        }
        [Authorize(Roles = Models.Constants.UserRoles.Admin + "," + Models.Constants.UserRoles.Reviewer)]
        // GET: Articles
        public ActionResult ReviewerArticles()
        {
            var myUserId = User.Identity.GetUserId();
            var articles = db.Articles.Where(a => a.IsReadyToReview).Include(a => a.AspNetUser).Include(a => a.AspNetUser1);
            return View("Index", articles.ToList());
        }
        [Authorize(Roles = Models.Constants.UserRoles.Admin)]
        // GET: Articles
        public ActionResult NewArticles()
        {
            var myUserId = User.Identity.GetUserId();
            var articles = db.Articles.Where(a => a.IsReadyToReview == false).Include(a => a.AspNetUser).Include(a => a.AspNetUser1);
            return View("Index", articles.ToList());
        }
        [Authorize(Roles = Models.Constants.UserRoles.Admin)]
        // GET: Articles
        public ActionResult ArticlesWithReviews()
        {
            var myUserId = User.Identity.GetUserId();
            var articles = db.Articles.Where(a => a.IsReadyToReview && a.ArticleReviews.Count > 0 && !a.IsReadyToPublish).Include(a => a.AspNetUser).Include(a => a.AspNetUser1);
            return View("Index", articles.ToList());
        }
        [Authorize(Roles = Models.Constants.UserRoles.Admin + "," + Models.Constants.UserRoles.Reviewer + "," + Models.Constants.UserRoles.Reader)]
        // GET: Articles
        public ActionResult PublishedArticles()
        {
            var myUserId = User.Identity.GetUserId();
            var articles = db.Articles.Where(a => a.IsReadyToReview && a.ArticleReviews.Count > 0 && a.IsReadyToPublish).Include(a => a.AspNetUser).Include(a => a.AspNetUser1);
            return View("Index", articles.ToList());
        }

        // GET: Articles/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var myUserId = User.Identity.GetUserId();
            var myRoles = LayerBao.UserBao.GetRoles(myUserId);
            var onlyMyArticles = myRoles.Count() > 0;
            var articles = db.Articles.Where(a => a.Id == id).Include(a => a.AspNetUser).Include(a => a.AspNetUser1);
            if (onlyMyArticles)
                articles = articles.Where(a => a.CreatedBy == myUserId);
            if (myRoles.Contains(Models.Constants.UserRoles.Reviewer))
            {
                articles = articles.Where(a => a.CreatedBy == myUserId || a.IsReadyToPublish || (a.IsReadyToReview && a.ArticleReviews.Count() == 0));
            }
            else if (myRoles.Contains(Models.Constants.UserRoles.Reader))
            {
                articles = articles.Where(a => a.CreatedBy == myUserId || a.IsReadyToPublish);
            }
            Article article = articles.FirstOrDefault();
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            //ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            //ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "Id,Title,SubTitle")] */Article article)
        {
            if (article != null && !string.IsNullOrWhiteSpace(article.Title))
            {
                article.CreatedBy = User.Identity.GetUserId();
                article.ModifiedBy = User.Identity.GetUserId();
                article.OnCreated = DateTime.Now;
                article.OnModified = DateTime.Now;
                article.Guid = Guid.NewGuid().ToString();
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email", article.ModifiedBy);
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", article.CreatedBy);
            return View(article);
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email", article.ModifiedBy);
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", article.CreatedBy);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,SubTitle")] Article article)
        {
            if (ModelState.IsValid)
            {
                article.ModifiedBy = User.Identity.GetApplicationUser().Id;
                article.OnModified = DateTime.Now;
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email", article.ModifiedBy);
            //ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", article.CreatedBy);
            return View(article);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult MarkArticleForReview(long id)
        {
            bool result = LayerBao.ArticlesBao.MarkForReview(id);
            return RedirectToAction("Details", "Articles", new { id });
        }
        [Authorize(Roles = "Admin")]
        public ActionResult MarkArticleForPublish(long id)
        {
            bool result = LayerBao.ArticlesBao.MarkForPublish(id);
            return RedirectToAction("Details", "Articles", new { id });
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
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
