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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace HospitalStaffManagement.Controllers
{
    [Authorize]

    public class staffsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        // GET: staffs

 
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ActionResult Index(Gender? GenderType, staffs staffs, string searchString)
        {
            var firstUser = db.Users.Where(s=>s.UserName == "Admin").FirstOrDefault();
            if (GenderType != null)
            {


                var sP = db.Users.Where(s => s.Gender == GenderType && s.Id != firstUser.Id).ToList();
                return View(sP);
            }
            if (!String.IsNullOrEmpty(searchString))
            {             
                var sP = db.Users.Where(s => s.Name.Contains(searchString)  || s.Mobile == searchString || s.LastName.Contains(searchString)).Where(s=> s.Id != firstUser.Id).ToList();
                return View(sP);
            }
            var items = db.Users.Where(s=> s.Id != firstUser.Id);
            return View(items.ToList());
        }

        public ActionResult Create()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<ActionResult>  Create(staffs staffs, HttpPostedFileBase Main_Image)
        {
            if (ModelState.IsValid)
            {
                if(Main_Image != null)
                {

                if (!(Main_Image.ContentType == "image/jpeg" || Main_Image.ContentType == "image/png" || Main_Image.ContentType == "image/bmp"))
                {
                    ModelState.AddModelError("", "نوع فایل غیر مجاز است");
                    return RedirectToAction(nameof(Create));
                }
                var name = Guid.NewGuid().ToString().Replace('-', '0') + "." + Main_Image.FileName.Split('.')[1];
                var imageUrl = "/Upload/Staffs/" + name;
                string path = Server.MapPath(imageUrl);
                Main_Image.SaveAs(path);
                staffs.Url = name;
                }
                var user = new ApplicationUser { UserName = staffs.UserName, LastName = staffs.LastName, Name = staffs.Name, Email = "" + staffs.UserName + "@yahoo.com", NationalCode = staffs.NationalCode, PasswordHash = staffs.Password, DecriptPass = staffs.Password, Mobile = staffs.Mobile, Gender = staffs.Gender, Address = staffs.Address , Url = staffs.Url };
                var result = await UserManager.CreateAsync(user, staffs.Password);
                if (result.Succeeded)
                {
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                    UserManager.AddToRole(user.Id, "Normal");
                }
                return RedirectToAction("Index");
            }     
            return View(staffs);
        }
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
         var   item  =  db.Users.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }
        [HttpPost]
        public  ActionResult  Edit(staffs staffs, HttpPostedFileBase Main_Image)
        {
        
            var item = db.Users.Where(s => s.Id == staffs.Id).FirstOrDefault();
            if (!string.IsNullOrEmpty(staffs.Name) && string.IsNullOrEmpty(staffs.LastName) == false && string.IsNullOrEmpty(staffs.Mobile) == false && string.IsNullOrEmpty(staffs.NationalCode) == false)
            {
                if (Main_Image != null)
                {
                    System.IO.File.Delete(Server.MapPath(String.Concat("~/Upload/Staffs/", item.Url)));
                    if (!(Main_Image.ContentType == "image/jpeg" || Main_Image.ContentType == "image/png" || Main_Image.ContentType == "image/bmp"))
                    {
                        ModelState.AddModelError("", "نوع فایل غیر مجاز است");
                        return RedirectToAction(nameof(Create));
                    }
                    var name = Guid.NewGuid().ToString().Replace('-', '0') + "." + Main_Image.FileName.Split('.')[1];
                    var imageUrl = "/Upload/Staffs/" + name;

                    string path = Server.MapPath(imageUrl);
                    Main_Image.SaveAs(path);
                    item.Url = name;
                }
                item.Name = staffs.Name;
                item.LastName = staffs.LastName;
                item.Mobile = staffs.Mobile;
                item.Address = staffs.Address;
               item.NationalCode = staffs.NationalCode;
           
                db.SaveChanges();
                return RedirectToAction("Index");
            }
     
            return View(item);
        }
        public  ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item =  db.Users.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var item = db.Users.Find(id);
            var haspic = db.Users.Where(s => s.Id == id).FirstOrDefault();
            if (haspic.Url != null)
            {
                System.IO.File.Delete(Server.MapPath(String.Concat("~/Upload/Staffs/", haspic.Url)));
            }
            db.Users.Remove(item);
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
