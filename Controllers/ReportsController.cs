using HospitalStaffManagement.Models;
using HospitalStaffManagement.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospitalStaffManagement.Controllers
{
    public class ReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reports
        public ActionResult Index(string TakeOffDate1 , string TakeOffDate2 , int? Gender , int? State ,int? ReportType ,string txtsearch)
        {
            string[] formats = { "yyyy-MMM-dd" };
            DateTime dt;
            if (!string.IsNullOrEmpty(TakeOffDate1))
            {
                if (!DateTime.TryParse(TakeOffDate1, out dt))
                {
                    ViewData["Error"] = "فرمت تاریخ صحیح نیست";
                    return View(db.Patients.ToList());
                }
            }
            if (!string.IsNullOrEmpty(TakeOffDate2))
            {
                if (!DateTime.TryParse(TakeOffDate2, out dt))
                {
                    ViewData["Error"] = "فرمت تاریخ صحیح نیست";
                    return View(db.Patients.ToList());
                }
            }
 

            var result = db.Patients.ToList();
            if (string.IsNullOrEmpty(TakeOffDate1) == false && string.IsNullOrEmpty(TakeOffDate2) == false)
            {
                DateTime FromDate = DateChanger.ToGeorgianDateTime(TakeOffDate1);
                DateTime ToDate = DateChanger.ToGeorgianDateTime(TakeOffDate2);
                result = result.Where(s => s.CreateDate >= FromDate && s.CreateDate <= ToDate).ToList();

                if (Gender != 3)
                {
                    var genderitem = (Gender)Gender;
                    result = result.Where(s => s.gender == genderitem).ToList();
                }
                if (State != 3)
                {
                    var stateitem = (State)State;
                    result = result.Where(s => s.State == stateitem).ToList();
                }
                if (ReportType != 3)
                {
                    if (ReportType == 0 && string.IsNullOrEmpty(txtsearch) == false)
                    {
                        result = result.Where(s => s.PrescribedDrugs.Contains(txtsearch)).ToList();
                    }
                    if (ReportType == 1 && string.IsNullOrEmpty(txtsearch) == false)
                    {
                        result = result.Where(s => s.DiseaseRecords.Contains(txtsearch)).ToList();
                    }
                }
                if (ReportType == 3 && string.IsNullOrEmpty(txtsearch) == false)
                {
                    result = result.Where(s => s.Name.Contains(txtsearch) || s.LastName.Contains(txtsearch) || s.Mobile == txtsearch).ToList();
                }

                ViewBag.count = "نتایج جستجو از این گزارش " + result.Count() + " مورد است .";
            }
            return View(result);
        }


        public ActionResult Statics()
        {
            ReportViewMode model = new ReportViewMode();
            model.StafCount = db.Users.Count() ;
            model.PatientCount = db.Patients.Count();
            model.GoodCount = db.Patients.Where(s => s.State == State.Good).Count();
            model.BadCount = db.Patients.Where(s => s.State == State.Bad).Count();
            model.WarningCount = db.Patients.Where(s => s.State == State.Alert).Count();
            model.MenCount= db.Patients.Where(s => s.gender == Gender.Male).Count();
            model.WomanCount = db.Patients.Where(s => s.gender == Gender.Female).Count();



            return View(model);
        }
    }
}