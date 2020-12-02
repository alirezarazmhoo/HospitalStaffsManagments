using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HospitalStaffManagement.Models;

namespace HospitalStaffManagement.Controllers
{
    public class SymptomsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public  ActionResult Index(string searchString)
        {
            if (searchString != null && searchString != "")
            {
                var items = db.Symptoms.Where(s => s.Name.Contains(searchString));
                return View(items.ToList());
            }
            var items2 = db.Symptoms;
            return View(items2.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public  ActionResult Create(Symptoms symptoms)
        {
            if (ModelState.IsValid)
            {
          
                db.Symptoms.Add(symptoms);
                 db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(symptoms);
        }
        // GET: Admin/Cities/Edit/5
        public  ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          var  item  =  db.Symptoms.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        [HttpPost]
        public ActionResult Edit(Symptoms symptoms)
        {
            if (ModelState.IsValid)
            {
                db.Entry(symptoms).State = EntityState.Modified;
                 db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(symptoms);
        }
        public  ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item =  db.Symptoms.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        [HttpPost, ActionName("Delete")]
        public  ActionResult DeleteConfirmed(int id)
        {
            var item =  db.Symptoms.Find(id);
            db.Symptoms.Remove(item);
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
