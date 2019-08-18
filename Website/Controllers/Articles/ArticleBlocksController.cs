using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LayerDb.Models;
using Website.Models;

namespace Website.Controllers.Articles
{
    public class ArticleBlocksController : Controller
    {
        private DbModel db = new DbModel();

        // GET: ArticleBlocks
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Articles");
            var articleBlocks = db.ArticleBlocks.Include(a => a.Article).Include(a => a.BlockType);
            return View(articleBlocks.ToList());
        }

        // GET: ArticleBlocks/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleBlock articleBlock = db.ArticleBlocks.Find(id);
            if (articleBlock == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Details", "Articles", new { id = articleBlock.ArticleId});
            //return View(articleBlock);
        }

        // GET: ArticleBlocks/Create
        public ActionResult Create(long id)
        {
            ViewBag.ArticleId = id;
            ViewBag.BlockTypeId = new SelectList(db.BlockTypes, "Id", "Name");
            return View();
        }

        // POST: ArticleBlocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ArticleId,BlockTypeId,Description")] ArticleBlock articleBlock,long id)
        {
            if (id > 0 && !string.IsNullOrWhiteSpace(articleBlock.Description) && articleBlock.BlockTypeId > 0)
            {
                articleBlock.ArticleId = id;
                articleBlock.CreatedBy = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(User.Identity);
                articleBlock.ModifiedBy = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(User.Identity);
                articleBlock.OnCreated = DateTime.Now;
                articleBlock.OnModified = DateTime.Now;
                db.ArticleBlocks.Add(articleBlock);
                db.SaveChanges();
                return RedirectToAction("Details", "Articles", new { id });
            }

            ViewBag.ArticleId = id;
            ViewBag.BlockTypeId = new SelectList(db.BlockTypes, "Id", "Name", articleBlock.BlockTypeId);
            return View(articleBlock);
        }

        // GET: ArticleBlocks/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleBlock articleBlock = db.ArticleBlocks.Find(id);
            if (articleBlock == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title", articleBlock.ArticleId);
            ViewBag.BlockTypeId = new SelectList(db.BlockTypes, "Id", "Name", articleBlock.BlockTypeId);
            return View(articleBlock);
        }

        // POST: ArticleBlocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ArticleId,BlockTypeId,Description,OnCreated,OnModified,CreatedBy,ModifiedBy")] ArticleBlock articleBlock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articleBlock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title", articleBlock.ArticleId);
            ViewBag.BlockTypeId = new SelectList(db.BlockTypes, "Id", "Name", articleBlock.BlockTypeId);
            return View(articleBlock);
        }

        // GET: ArticleBlocks/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleBlock articleBlock = db.ArticleBlocks.Find(id);
            if (articleBlock == null)
            {
                return HttpNotFound();
            }
            return View(articleBlock);
        }

        // POST: ArticleBlocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ArticleBlock articleBlock = db.ArticleBlocks.Find(id);
            db.ArticleBlocks.Remove(articleBlock);
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
