using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace HospitalStaffManagement.Models
{
	public class staffs
	{
		public string Id { get; set; }

		[Required(ErrorMessage ="نام را وارد کنید")]
		[DisplayName("نام")]
		public string Name { get; set; }
		[Required(ErrorMessage = "نام خانوادگی را وارد کنید")]
		[DisplayName("نام خانوادگی")]
		public string LastName { get; set; }
		[Required(ErrorMessage = "شماره همراه را وارد کنید")]
		[DisplayName("شماره همراه")]
		public string Mobile { get; set; }
		[DisplayName("کدملی")]

		public string NationalCode { get; set; }
	
		[Required(ErrorMessage = "نام کاربری کارمند را وارد کنید")]
		[DisplayName("نام کاربری")]
		public string UserName { get; set; }
		[Required(ErrorMessage = " رمز عبور کارمند را وارد کنید")]
		[DisplayName("رمز عبور")]
		public string Password { get; set; }
		public string Address { get; set; }
		public Gender Gender { get; set; }
		[DisplayName("عکس پرسنلی")]
		public string Url { get; set; }
	}

	public enum Gender
	{
		Male , 
		Female
	}

}