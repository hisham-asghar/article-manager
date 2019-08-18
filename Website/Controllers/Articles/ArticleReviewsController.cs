using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LayerDb.Models;
using Microsoft.AspNet.Identity;

namespace Website.Controllers.Articles
{
    public class ArticleReviewsController : Controller
    {
        private DbModel db = new DbModel();

        // GET: ArticleReviews
        public ActionResult Index()
        {
            var articleReviews = db.ArticleReviews.Include(a => a.Article).Include(a => a.AspNetUser);
            return View(articleReviews.ToList());
        }

        // GET: ArticleReviews/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleReview articleReview = db.ArticleReviews.Find(id);
            if (articleReview == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Details", "Articles", new { id = articleReview.ArticleId });
            //return View(articleReview);
        }

        // GET: ArticleReviews/Create
        public ActionResult Create(long id)
        {
            //ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title");
            //ViewBag.ReviewerId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: ArticleReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ReviewerId,Description")] ArticleReview articleReview, long id)
        {
            if (id > 0 && !string.IsNullOrWhiteSpace(articleReview.Description))
            {
                articleReview.ArticleId = id;
                articleReview.ReviewerId = User.Identity.GetUserId();
                articleReview.CreatedBy = User.Identity.GetUserId();
                articleReview.ModifiedBy = User.Identity.GetUserId();
                articleReview.OnCreated = DateTime.Now;
                articleReview.OnModified = DateTime.Now;
                db.ArticleReviews.Add(articleReview);
                db.SaveChanges();
                return RedirectToAction("Details", "Articles", new { id });
            }

            return View(articleReview);
        }

        // GET: ArticleReviews/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleReview articleReview = db.ArticleReviews.Find(id);
            if (articleReview == null)
            {
                return HttpNotFound();
            }
            return View(articleReview);
        }

        // POST: ArticleReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ReviewerId,ArticleId,Description,OnCreated,OnModified,CreatedBy,ModifiedBy")] ArticleReview articleReview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articleReview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title", articleReview.ArticleId);
            ViewBag.ReviewerId = new SelectList(db.AspNetUsers, "Id", "Email", articleReview.ReviewerId);
            return View(articleReview);
        }

        // GET: ArticleReviews/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleReview articleReview = db.ArticleReviews.Find(id);
            if (articleReview == null)
            {
                return HttpNotFound();
            }
            return View(articleReview);
        }

        // POST: ArticleReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ArticleReview articleReview = db.ArticleReviews.Find(id);
            db.ArticleReviews.Remove(articleReview);
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
