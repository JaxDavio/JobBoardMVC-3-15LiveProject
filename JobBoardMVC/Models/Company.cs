using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobBoardMVC.Models
{
    public class Company
    {
        [Key]
		[Display(Name = "Company Name")]
        public string CompanyName { get; set; }

		[Display(Name = "Company Link")]
		public string CompanyLink { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }

		[Display(Name = "Zip Code")]
		public string ZipCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

		[Display(Name = "Career Page")]
		public string CareerPage { get; set; }
        public string Description { get; set; }
        public string Categories { get; set; }

        public string JobCode { get; set; }

        public string LocationCode { get; set; }

		[Display(Name = "Company Type")]
		public string CompanyType { get; set; }

		[Display(Name = "Industry Served")]
		public string IndustryServed { get; set; }

		[Display(Name = "Languages Used")]
		public string LanguagesUsed { get; set; }

		public virtual ICollection<CompanyFile> Files { get; set; }
	}
}
