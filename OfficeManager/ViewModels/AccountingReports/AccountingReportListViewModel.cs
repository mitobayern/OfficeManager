using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace OfficeManager.ViewModels.AccountingReports
{
    public class AccountingReportListViewModel
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public DateTime CreatedOn { get; set; }

        //public string CreatedOn { get; set; }


        public string  Period { get; set; }

        public string CompanyName { get; set; }

        public decimal TotalAmount { get; set; }

        //public List<SelectListItem> AllPeriods { get; set; }

        //public List<SelectListItem> AllTenants { get; set; }

        //public List<string> AllTenants { get; set; }

        //public List<string> AllPeriods { get; set; }


        //public string TotalAmount { get; set; }

    }
}
