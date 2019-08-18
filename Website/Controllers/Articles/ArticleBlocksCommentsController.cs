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
    public class ArticleBlocksCommentsController : Controller
    {
        private DbModel db = new DbModel();

        // GET: ArticleBlocksComments
        public ActionResult Index()
        {
            var articleBlocksComments = db.ArticleBlocksComments.Include(a => a.ArticleBlock);
            return View(articleBlocksComments.ToList());
        }

        // GET: ArticleBlocksComments/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleBlocksComment articleBlocksComment = db.ArticleBlocksComments.Find(id);
            if (articleBlocksComment == null)
            {
                return HttpNotFound();
            }
            return View(articleBlocksComment);
        }

        // GET: ArticleBlocksComments/Create
        public ActionResult Create(long id)
        {
            //ViewBag.ArticleBlockId = new SelectList(db.ArticleBlocks, "Id", "Description");
            return View();
        }

        // POST: ArticleBlocksComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description")] ArticleBlocksComment articleBlocksComment,long id)
        {
            if (articleBlocksComment != null && id > 0)
            {
                articleBlocksComment.ArticleBlockId = id;
                articleBlocksComment.CreatedBy = User.Identity.GetUserId();
                articleBlocksComment.ModifiedBy = User.Identity.GetUserId();
                articleBlocksComment.OnCreated = DateTime.Now;
                articleBlocksComment.OnModified = DateTime.Now;
                db.ArticleBlocksComments.Add(articleBlocksComment);
                db.SaveChanges();
                return RedirectToAction("Details", "ArticleBlocks", new { id });
            }

            ViewBag.ArticleBlockId = new SelectList(db.ArticleBlocks, "Id", "Description", articleBlocksComment.ArticleBlockId);
            return View(articleBlocksComment);
        }

        // GET: ArticleBlocksComments/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleBlocksComment articleBlocksComment = db.ArticleBlocksComments.Find(id);
            if (articleBlocksComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArticleBlockId = new SelectList(db.ArticleBlocks, "Id", "Description", articleBlocksComment.ArticleBlockId);
            return View(articleBlocksComment);
        }

        // POST: ArticleBlocksComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ArticleBlockId,Description,OnCreated,OnModified,CreatedBy,ModifiedBy")] ArticleBlocksComment articleBlocksComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articleBlocksComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArticleBlockId = new SelectList(db.ArticleBlocks, "Id", "Description", articleBlocksComment.ArticleBlockId);
            return View(articleBlocksComment);
        }

        // GET: ArticleBlocksComments/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleBlocksComment articleBlocksComment = db.ArticleBlocksComments.Find(id);
            if (articleBlocksComment == null)
            {
                return HttpNotFound();
            }
            return View(articleBlocksComment);
        }

        // POST: ArticleBlocksComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ArticleBlocksComment articleBlocksComment = db.ArticleBlocksComments.Find(id);
            db.ArticleBlocksComments.Remove(articleBlocksComment);
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
