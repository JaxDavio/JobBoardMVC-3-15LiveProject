using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace JobBoardMVC.Models
{
    public class Job
    {
        public int ID { get; set; }

        [JsonProperty(PropertyName = "ApplicationLink")]
        [Display(Name = "Application Link")]
        public string ApplicationLink { get; set; }
        
        [JsonProperty(PropertyName = "CompanyName")]
        [Display(Name = "Company")]
        public string CompanyCompanyName { get; set; }
       
        [JsonProperty(PropertyName = "DatePosted")]
        [Display(Name = "Date Posted")]
        public string DatePosted { get; set; }

        [JsonProperty(PropertyName = "Experience")]
        public string Experience { get; set; }

        [JsonProperty(PropertyName = "Hours")]
        public string Hours { get; set; }

        [JsonProperty(PropertyName = "JobID")]
        [Display(Name = "Job ID")]
        public string JobID { get; set; }

        [JsonProperty(PropertyName = "JobTitle")]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [JsonProperty(PropertyName = "LanguagesUsed")]
        [Display(Name = "Languages Used")]
        public string LanguagesUsed { get; set; }

        [JsonProperty(PropertyName = "Location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "Salary")]
        public string Salary { get; set; }

        [JsonIgnore]//possible fix for api error, api can run but cannot merge jobs database with company database
        public virtual Company Company {get; set;}//also being used for tooltip on Jobs Index


        //possible fix for api error, fully solve api
        //public Company Company { get; set; }
    }
}


        
