using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobBoardMVC.Models
{
    public class CompanyViewModel
    {
        public Company company { get; set; }
        public IEnumerable<Job> jobs { get; set; }
        public int jobCount { get; set; }
        public bool companySaved { get; set; }
    }
}