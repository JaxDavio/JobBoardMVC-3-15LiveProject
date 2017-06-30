using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobBoardMVC.Models
{
    public class SavedCompany
    {
        public int ID { get; set; }

        [Display(Name = "User ID")]
        public Guid UserID { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyCompanyName { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public virtual Company Company { get; set; }
    }
}