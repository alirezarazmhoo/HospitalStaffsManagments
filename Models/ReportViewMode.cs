using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalStaffManagement.Models
{
	public class ReportViewMode
	{

		public int StafCount { get; set; }
		public int PatientCount { get; set; }
		public int GoodCount { get; set; }
		public int WarningCount { get; set; }
		public int BadCount { get; set; }
		public int MenCount { get; set; }
		public int WomanCount { get; set; }



	}
}