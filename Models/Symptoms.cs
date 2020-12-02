using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalStaffManagement.Models
{
	public class Symptoms
	{
		public int Id { get; set; }
		[Required(ErrorMessage =" نشانه بیماری را وارد کنید")]
		[DisplayName("نشانه بیماری")]
		public string Name { get; set; }
	}
}