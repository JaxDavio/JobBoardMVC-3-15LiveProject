using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobBoardMVC.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using JobBoardMVC.CustomFilters;
using System.Net;
using System.Net.Mail;

namespace JobBoardMVC.Controllers
{
    public class HomeController : Controller
    {
        private JobBoardMvcContext context = new JobBoardMvcContext();
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "JobPDX";

            return View();
        }

        public ActionResult Privacy()
        {
            ViewBag.Message = "Privacy page";

            return View();
        }

        public ActionResult TOS()
        {
            ViewBag.Message = "Terms of Service page";

            return View();
        }

        public ActionResult FAQ()
        {
            ViewBag.Message = "Frequently Asked Questions";

            return View();
        }

        // GET: Job Count & Company Count for Non-Authenticated & Non-Admin Users

        public async Task<ActionResult> Index(string jobTitleString, string companyString, int selectedLocationId = 0, int JobID = 0)
        {
			// Include LINQ queries to allow filters
			var jobs = from j in context.Jobs
                       select j;

            // Var for counting number of companies used below in company count
            var companies = from c in context.Companies
                            select c;


            // Job title search form field
            if (!String.IsNullOrEmpty(jobTitleString))
            {
                jobs = jobs.Where(j => j.JobTitle.Contains(jobTitleString));
            }

            // company search form field
            if (!String.IsNullOrEmpty(companyString))
            {
                jobs = jobs.Where(j => j.CompanyCompanyName.Contains(companyString));
            }

            // grab a count of the number of jobs currently inside the jobs variable.
            int count = jobs.Count();
            // store it in viewbag for the View to display
            ViewBag.Counts = count;

            // Set JobID for indication whether Index action was called from Save action
            ViewBag.JobID = JobID;

            // Need for company count
            if (!String.IsNullOrEmpty(companyString))
            {
                companies = companies.Where(c => c.CompanyName.Contains(companyString));
            }
            // Grab a count of the number of companies inside the companies variable
            int companyCount = companies.Count();
            // store it in viewbag for the View to display
            ViewBag.companyCount = companyCount;

            //instantiate the view model and serve it to the view
                       
            var jobLocationVM = new JobLocationViewModel();
            jobLocationVM.jobs = await jobs.ToListAsync();

            return View(jobLocationVM);
            
        }

        // GET: Admin
        // This section needs Admin Authentication
        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Admin(string jobTitleString, string companyString, int selectedLocationId = 0)
        {
            // Include LINQ queries to allow filters
            var jobs = from j in context.Jobs
                       select j;

            // adding a var for counting number of companies used below in company count
            var companies = from c in context.Companies
                            select c;


            // Job title search form field
            if (!String.IsNullOrEmpty(jobTitleString))
            {
                jobs = jobs.Where(j => j.JobTitle.Contains(jobTitleString));
            }

            // company search form field
            if (!String.IsNullOrEmpty(companyString))
            {
                jobs = jobs.Where(j => j.CompanyCompanyName.Contains(companyString));
            }

            // grab a count of the number of jobs currently inside the jobs variable.
            int count = jobs.Count();
            // store it in viewbag for the View to display
            ViewBag.Counts = count;

            // Need for company count
            if (!String.IsNullOrEmpty(companyString))
            {
                companies = companies.Where(c => c.CompanyName.Contains(companyString));
            }
            // Grab a count of the number of companies inside the companies variable
            int companyCount = companies.Count();
            // store it in viewbag for the View to display
            ViewBag.companyCount = companyCount;

            //instantiate the view model and serve it to the view

            var jobLocationVM = new JobLocationViewModel();
            jobLocationVM.jobs = await jobs.ToListAsync();

            return View(jobLocationVM);

        }
        //Contact form information and message transmission
        //Future changes to email destination must also be made in Web.config
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(ContactFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Telephone Number: {2}</p><p>Message:</p><p>{3}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("jobpdxforwarding@gmail.com"));   
                message.From = new MailAddress(model.FromEmail.ToString());  
                message.Subject = "JobPDX Contact message";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.PhoneNumber, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
        }
        public ActionResult Sent()
        {
            return View();
        }
    }
}
