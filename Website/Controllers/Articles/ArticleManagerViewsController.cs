using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LayerDb.Models;
using LayerBao;
namespace Website.Controllers.Articles
{
    public class ArticleManagerViewsController : Controller
    {
        private DbModel db = new DbModel();
      
        // GET: ArticleManagerViews
        public ActionResult Index(string id)
        {
            var list = LayerBao.ArticlesBao.GetArticleManagerView(id ?? Models.Constants.GetDefaultDatabase());
            return View(list);
        }

        // GET: ArticleManagerViews/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleManagerView articleManagerView = db.ArticleManagerViews.Find(id);
            if (articleManagerView == null)
            {
                return HttpNotFound();
            }
            return View(articleManagerView);
        }

        // GET: ArticleManagerViews/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ArticleManagerViews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,OnCreated,OnModified,CreatedBy,ModifiedBy,IsReadyToPublish,IsReadyToReview,IsDeleted,SubTitle,Guid,ReviewCount,CreatedByName,CreatedByEmail")] ArticleManagerView articleManagerView)
        {
            if (ModelState.IsValid)
            {
                db.ArticleManagerViews.Add(articleManagerView);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(articleManagerView);
        }

        // GET: ArticleManagerViews/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleManagerView articleManagerView = db.ArticleManagerViews.Find(id);
            if (articleManagerView == null)
            {
                return HttpNotFound();
            }
            return View(articleManagerView);
        }

        // POST: ArticleManagerViews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,OnCreated,OnModified,CreatedBy,ModifiedBy,IsReadyToPublish,IsReadyToReview,IsDeleted,SubTitle,Guid,ReviewCount,CreatedByName,CreatedByEmail")] ArticleManagerView articleManagerView)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articleManagerView).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(articleManagerView);
        }

        // GET: ArticleManagerViews/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleManagerView articleManagerView = db.ArticleManagerViews.Find(id);
            if (articleManagerView == null)
            {
                return HttpNotFound();
            }
            return View(articleManagerView);
        }

        // POST: ArticleManagerViews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ArticleManagerView articleManagerView = db.ArticleManagerViews.Find(id);
            db.ArticleManagerViews.Remove(articleManagerView);
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
        public ActionResult ReviewRequest(int? id)
        {
            ArticleManagement.ReviewRequest(id);
            return RedirectToAction("Index");

        }
        public ActionResult ArticleReviewed(int? id)
        {
            ArticleManagement.ArticleReviewed(id);
            return RedirectToAction("Index");

        }
        public ActionResult PublishRequesh(int? id)
        {
            ArticleManagement.PublishRequest(id);
            return RedirectToAction("Index");

        }
        public ActionResult ArticlePublished(int? id)
        {
            ArticleManagement.ArticlePublished(id);
            return RedirectToAction("Index");

        }
    }
}
