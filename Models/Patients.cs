using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalStaffManagement.Models
{
	public class Patients
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "نام را وارد کنید")]
		[DisplayName("نام")]
		public string Name { get; set; }
		[Required(ErrorMessage = "نام خانوادگی را وارد کنید")]
		[DisplayName("نام خانوادگی")]
		public string LastName { get; set; }
		public Gender gender { get; set; }
		[Required(ErrorMessage = "شماره همراه را وارد کنید")]
		[DisplayName("شمار همراه")]
		public string Mobile { get; set; }
		[DisplayName("سن")]
		[Required(ErrorMessage = "سن بیمار را وارد کنید")]
		public int old { get; set; }

		[DisplayName("عکس بیمار")]
		public string Url { get; set; }
		public State State { get; set; }
		[DisplayName("آدرس محل سکونت")]
		public string Address { get; set; }
		[DisplayName("علائم بیماری")]
		public string DiseaseRecords { get; set; }
		[DisplayName("داروی های تجویز شده")]
		public string PrescribedDrugs { get; set; }
		[DisplayName("کارمند ثبت کننده")]
		public string StaffCreatorFullName { get; set; }
		[DisplayName("تاریخ ایجاد")]
		public DateTime CreateDate { get; set; }

		public string StaffCreatorId { get; set; }
		[DisplayName("سوابق بیماری")]
		public string DossierSickness { get; set; }



	}

	public enum State
	{
		Bad , 
		Alert , 
		Good 
	}

}