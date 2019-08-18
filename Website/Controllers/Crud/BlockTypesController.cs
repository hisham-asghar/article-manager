using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LayerDb.Models;

namespace Website.Controllers.Crud
{
    public class BlockTypesController : Controller
    {
        private DbModel db = new DbModel();

        // GET: BlockTypes
        public ActionResult Index()
        {
            return View(db.BlockTypes.ToList());
        }

        // GET: BlockTypes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlockType blockType = db.BlockTypes.Find(id);
            if (blockType == null)
            {
                return HttpNotFound();
            }
            return View(blockType);
        }

        // GET: BlockTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlockTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] BlockType blockType)
        {
            if (ModelState.IsValid)
            {
                db.BlockTypes.Add(blockType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blockType);
        }

        // GET: BlockTypes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlockType blockType = db.BlockTypes.Find(id);
            if (blockType == null)
            {
                return HttpNotFound();
            }
            return View(blockType);
        }

        // POST: BlockTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] BlockType blockType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blockType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blockType);
        }

        // GET: BlockTypes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlockType blockType = db.BlockTypes.Find(id);
            if (blockType == null)
            {
                return HttpNotFound();
            }
            return View(blockType);
        }

        // POST: BlockTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            BlockType blockType = db.BlockTypes.Find(id);
            db.BlockTypes.Remove(blockType);
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
