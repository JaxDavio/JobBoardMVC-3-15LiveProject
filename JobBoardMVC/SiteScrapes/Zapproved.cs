using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using Newtonsoft.Json;
using System;
using System.Reflection;
using JobBoardMVC.Models;

namespace JobBoardMVC.SiteScrapes
{
    public class Zapproved
    {
        public static void Scrape()
        {
            string company = "Zapproved";
            GetJobsData(company);

        }

        public static void GetJobsData(string company)
        {
            string url = "https://careers.jobscore.com/careers/zapproved";

            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc = web.Load(url);

            IEnumerable<HtmlNode> jobTitleAndLinkArray = doc.DocumentNode.CssSelect(".js-job-title");
            IEnumerable<HtmlNode> locationArray = doc.DocumentNode.CssSelect(".js-job-location");

            List<Job> jobs = new List<Job>();

            for (int i = 0; i < jobTitleAndLinkArray.Count(); i++)
            {
                Job job = new Job();

                HtmlNode titleAndLink = jobTitleAndLinkArray.ElementAt(i);
                HtmlNode location = locationArray.ElementAt(i);

                job.ApplicationLink = url + titleAndLink.ChildNodes[1].Attributes["href"].Value;
                job.CompanyCompanyName = company;
                job.DatePosted = DateTime.Now.ToString();
                job.Experience = "";
                job.Hours = "";
                job.JobID = "";
                job.JobTitle = System.Net.WebUtility.HtmlDecode(titleAndLink.ChildNodes[1].InnerText);
                job.LanguagesUsed = "";
                job.Location = location.InnerText.Replace("\n", "");
                job.Salary = "";

                jobs.Add(job);


            }

            JobBoardMVC.Controllers.ScrapesController.UploadScrapes(jobs, company);
        }
    }
}
