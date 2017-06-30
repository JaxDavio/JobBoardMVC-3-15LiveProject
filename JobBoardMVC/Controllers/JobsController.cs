using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobBoardMVC.Models;
using System.Data.Entity.Migrations;
using JobBoardMVC.CustomFilters;
using Microsoft.AspNet.Identity;

namespace JobBoardMVC.Controllers
{
    public class JobsController : Controller
    {
        private JobBoardMvcContext context = new JobBoardMvcContext();

        // GET: Jobs
  
        public async Task<ActionResult> Index(string jobTitleString, string companyString, int selectedLocationId = 0, int JobID = 0)
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

            // Create a list of jobIDs where the userID equals the current user
            // from the SavedJobs table. This is used to evaluate whether or not the
            // save button should be enabled or disabled in the job search results.
            if (User.Identity.GetUserId() != null)
            {
                var userID = Guid.Parse(User.Identity.GetUserId());
                IEnumerable<SavedJob> savedJobsView = context.SavedJobs.Where(s => s.UserID.Equals(userID));
                var usersSavedJobIDs = savedJobsView.Select(s => s.JobID).ToList();
                ViewBag.usersSavedJobIDs = usersSavedJobIDs;
            }


            //instantiate the view model and serve it to the view

            var jobLocationVM = new JobLocationViewModel();
            jobLocationVM.jobs = await jobs.ToListAsync();

            return View(jobLocationVM);
        }

        // GET: Jobs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await context.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Jobs/Create
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Create()
        {
			PopulateCompanyDropdownList();
			return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ApplicationLink,Company,DatePosted,Experience,Hours,JobID,JobTitle,LanguagesUsed,Location,Salary")] Job job)
        {
            if (ModelState.IsValid)
            {
                context.Jobs.Add(job);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
			PopulateCompanyDropdownList();
			return View(job);
        }

        //GET: Jobs/Edit/5
        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await context.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
			PopulateCompanyDropdownList();
			return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ApplicationLink,Company,DatePosted,Experience,Hours,JobID,JobTitle,LanguagesUsed,Location,Salary")] Job job)
        {
            if (ModelState.IsValid)
            {
                context.Entry(job).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
			PopulateCompanyDropdownList();
			return View(job);
        }

		private void PopulateCompanyDropdownList()
		{
			var companies = new List<string>();
			var companyQuery = from c in context.Companies
							   orderby c.CompanyName
							   select c.CompanyName;
			companies.AddRange(companyQuery.Distinct());
			ViewBag.Companies = new SelectList(companies);
		}

		//GET: Jobs/Delete/5
		[CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await context.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        //public ActionResult Delete()
        //{
        //    return View();
        //}

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Job job = await context.Jobs.FindAsync(id);
            context.Jobs.Remove(job);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }

        // Save - this action takes a jobId value passed to it from a save button on the index page
        //        then adds the associated JobID, UserId, applicationLink, Company, and JobTitle information
        //        to the SavedJobs table. It is called via ajax from the page, so it doesn't return a 
        //        view. It's possible, therefore, that the syntax of the action method should be changed
        //        at some point.
        
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Save(int? jobId)
        {
            if (jobId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Job job = await context.Jobs.FindAsync(jobId);
            if (job == null)
            {
                return HttpNotFound();
            }


            var userID = Guid.Parse(User.Identity.GetUserId());
            
            if (ModelState.IsValid)
            {
                var savedJob = new SavedJob();
                savedJob.JobID = job.JobID;
                savedJob.UserID = userID;
                savedJob.ApplicationLink = job.ApplicationLink;
                savedJob.CompanyCompanyName = job.CompanyCompanyName;
                savedJob.JobTitle = job.JobTitle;
                savedJob.DatePosted = job.DatePosted;
                savedJob.DateSaved = DateTime.Now;
                savedJob.Location = job.Location;
                context.SavedJobs.Add(savedJob);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        /*
        // CompanyInfo - 
        // CompanyName value is passed from index page, linked to Company.Id 
        // then builds a new tabbed page with CompanyName, Address, Map information
 
        [Authorize]
        public async Task<ActionResult> CompanyInfo(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Company company = await context.Companies.FindAsync(Id);
            if (company == null)
            {
                return HttpNotFound();
            }
            
            if (ModelState.IsValid)
            {
                //do we need a new model for the CompanyInfo Page?
                /*
                var company = new SavedJob();
                savedJob.UserID = userID;
                savedJob.ApplicationLink = job.ApplicationLink;
                savedJob.Company = job.CompanyName;
                savedJob.JobTitle = job.JobTitle;
                context.SavedJobs.Add(savedJob);
                await context.SaveChangesAsync();
                return View(context.SavedJobs.Where(p => p.UserID == userID).ToList());
                
            }

            return RedirectToAction("Index");
        }
*/
    }
}
