using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HospitalStaffManagement.Models;
using HospitalStaffManagement.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HospitalStaffManagement.Controllers
{
[Authorize]
    public class PatientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Patients
        public async Task<ActionResult> Index(Gender? GenderType , string searchString)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (GenderType != null)
            {
                if (User.IsInRole("Admin"))
                {
                    var sP1 = db.Patients.Where(s => s.gender == GenderType).ToList();
                    return View(sP1);
                }
                var sP = db.Patients.Where(s => s.gender == GenderType && s.StaffCreatorId == user.Id).ToList();
                return View(sP);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                if (User.IsInRole("Admin"))
                {
                    var sP2 = db.Patients.Where(s => s.Name.Contains(searchString) || s.Mobile == searchString || s.LastName.Contains(searchString)).ToList();
                    return View(sP2);
                }
                var sP = db.Patients.Where(s => s.Name.Contains(searchString) || s.Mobile == searchString || s.LastName.Contains(searchString)).Where(s=>s.StaffCreatorId == user.Id).ToList();
                return View(sP);
            }

            if (User.IsInRole("Admin"))
            {

                return View(db.Patients.ToList());

            }
            else
            {
                return View(db.Patients.Where(s => s.StaffCreatorId == user.Id).ToList());
            }

        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost] 
        public async Task<ActionResult>  Create(Patients patients , HttpPostedFileBase Main_Image)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
             
                if (Main_Image != null)
                {
                    if (!(Main_Image.ContentType == "image/jpeg" || Main_Image.ContentType == "image/png" || Main_Image.ContentType == "image/bmp"))
                    {
                        ViewData["Error"] = "فرمت عکس  صحیح نیست";
                        return View(patients);
                    }
                    var name = Guid.NewGuid().ToString().Replace('-', '0') + "." + Main_Image.FileName.Split('.')[1];
                    var imageUrl = "/Upload/Patients/" + name;
                    string path = Server.MapPath(imageUrl);
                    Main_Image.SaveAs(path);
                    patients.Url = name;
                }
                patients.CreateDate = DateTime.Now;
               
                patients.StaffCreatorFullName = user.Name + " " + user.LastName;
                patients.StaffCreatorId = user.Id;
                db.Patients.Add(patients);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patients);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patients patients = db.Patients.Find(id);
            if (patients == null)
            {
                return HttpNotFound();
            }
            return View(patients);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Patients patients , HttpPostedFileBase Main_Image)
        {
            var item = db.Patients.Where(s => s.Id == patients.Id).FirstOrDefault();

            if (!string.IsNullOrEmpty(patients.Name) && string.IsNullOrEmpty(patients.LastName) == false && string.IsNullOrEmpty(patients.Mobile) == false)
            {
                if (Main_Image != null)
                {
                    System.IO.File.Delete(Server.MapPath(String.Concat("~/Upload/Patients/", item.Url)));
                    if (!(Main_Image.ContentType == "image/jpeg" || Main_Image.ContentType == "image/png" || Main_Image.ContentType == "image/bmp"))
                    {
                        ViewData["Error"] = "فرمت عکس  صحیح نیست";
                        return RedirectToAction(nameof(Create));
                    }
                    var name = Guid.NewGuid().ToString().Replace('-', '0') + "." + Main_Image.FileName.Split('.')[1];
                    var imageUrl = "/Upload/Patients/" + name;
                    string path = Server.MapPath(imageUrl);
                    Main_Image.SaveAs(path);
                    item.Url = name;
                }
                item.Name = patients.Name;
                item.LastName = patients.LastName;
                item.Mobile = patients.Mobile;
                item.Address = patients.Address;
                item.old = patients.old;
                item.PrescribedDrugs = patients.PrescribedDrugs;
                item.DiseaseRecords = patients.DiseaseRecords;
                item.gender = patients.gender;
                item.State = patients.State;
                item.DossierSickness = patients.DossierSickness;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patients);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patients patients = db.Patients.Find(id);
            if (patients == null)
            {
                return HttpNotFound();
            }
            return View(patients);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Patients patients = db.Patients.Find(id);

            if (!string.IsNullOrEmpty(patients.Url))
            {
                System.IO.File.Delete(Server.MapPath(String.Concat("~/Upload/Patients/", patients.Url)));
            }
            db.Patients.Remove(patients);
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
