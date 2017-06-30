using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobBoardMVC.Models;
using Microsoft.AspNet.Identity;
using JobBoardMVC.CustomFilters;
using System.Threading.Tasks;
using System.Drawing;

namespace JobBoardMVC
{
    public class CompanyInfoController : Controller
    {
        private JobBoardMvcContext db = new JobBoardMvcContext();

		public ActionResult Index(int id)
		{
			var fileToRetrieve = db.CompanyFiles.Find(id);
			return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
		}

		public ActionResult Details(string id)
        {
            var model = new CompanyViewModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            model.company = db.Companies.Include(c => c.Files).SingleOrDefault(c => c.CompanyName == id);
            if (model.company == null)
            {
                return HttpNotFound();
            }
            // model.jobs
            model.jobs = db.Jobs.Where(j => j.CompanyCompanyName == model.company.CompanyName).ToList();
            //<List>model.jobs = <List>db.Jobs.Where(j => j.CompanyCompanyName == model.company.CompanyName);

            model.jobCount = model.jobs.ToList().Count;

            // check if this company has already been saved by this user
            var userID = Guid.Parse(User.Identity.GetUserId());
            var saved = db.SavedCompanies.Where(s => s.CompanyCompanyName == model.company.CompanyName && s.UserID == userID).FirstOrDefault();
            model.companySaved = (saved == null ? false : true);

            // Create a list of jobIDs where the userID equals the current user
            // from the SavedJobs table. This is used to evaluate whether or not the
            // save button should be enabled or disabled in the company info results.
            IEnumerable<SavedJob> savedJobsView = db.SavedJobs.Where(s => s.UserID.Equals(userID));
            var usersSavedJobIDs = savedJobsView.Select(s => s.JobID).ToList();
            ViewBag.usersSavedJobIDs = usersSavedJobIDs;

            return View(model);
        }


		// GET: CompanyInfo/Create
		[CustomAuthorize(Roles = "Admin")]
		public ActionResult Create()
		{
			return View();
		}

		// POST: CompanyInfo/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[CustomAuthorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "CompanyName,CompanyLink,Address,City,State,ZipCode,Latitude,Longitude,CareerPage,CompanyType,IndustryServed,LanguagesUsed,Files")] Company company, HttpPostedFileBase upload)
		{
			var _company = db.Companies.Find(company.CompanyName);
			if (_company != null)
			{
				ViewBag.ErrorMessage = company.CompanyName + " already exists.";
			}
			else if (ModelState.IsValid)
			{
				if (upload != null && upload.ContentLength > 0)
				{
					var logo = new CompanyFile
					{
						FileName = System.IO.Path.GetFileName(upload.FileName),
						FileType = FileType.Logo,
						ContentType = upload.ContentType,
						CompanyName = company.CompanyName
					};
					using (var reader = new System.IO.BinaryReader(upload.InputStream))
					{
						logo.Content = reader.ReadBytes(upload.ContentLength);
					}
					company.Files = new List<CompanyFile> { logo };
				}
				db.Companies.Add(company);
				await db.SaveChangesAsync();
				return RedirectToAction("Details", new { id = company.CompanyName });
			}

			return View(company);
		}

		//GET: Jobs/Edit/5
		[CustomAuthorize(Roles = "Admin")]
		public ActionResult Edit(string id)
		{
			var company = new Company();
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			company = db.Companies.Include(c => c.Files).SingleOrDefault(c => c.CompanyName == id);
			if (company == null)
			{
				return HttpNotFound();
			}
			return View(company);
		}

		// POST: Jobs/Edit/5
		[HttpPost]
		[CustomAuthorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(string id, HttpPostedFileBase upload)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var companyToUpdate = db.Companies.Find(id);
			try
			{
				if (upload != null && upload.ContentLength > 0)
				{
					var oldLogo = companyToUpdate.Files.Any(f => f.FileType == FileType.Logo);
					if (oldLogo == true)
					{
						db.CompanyFiles.Remove(companyToUpdate.Files.First(f => f.FileType == FileType.Logo));
					}
					var logo = new CompanyFile
					{
						FileName = System.IO.Path.GetFileName(upload.FileName),
						FileType = FileType.Logo,
						ContentType = upload.ContentType,
						CompanyName = companyToUpdate.CompanyName
					};
					using (var reader = new System.IO.BinaryReader(upload.InputStream))
					{
						logo.Content = reader.ReadBytes(upload.ContentLength);
					}
					companyToUpdate.Files = new List<CompanyFile> { logo };
				}
				if (ModelState.IsValid && TryUpdateModel(companyToUpdate, new string[] { "CompanyName", "CompanyLink", "Address", "City", "State", "ZipCode", "Latitude", "Longitude", "CareerPage", "CompanyType", "IndustryServed", "LanguagesUsed", "Files" }))
				{
					try
					{
						db.SaveChanges();
						return RedirectToAction("Details", new { id = companyToUpdate.CompanyName });
					}
					catch (DataException /*ex*/)
					{
						ModelState.AddModelError("", "Unable to save changes.");
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return View(companyToUpdate);
		}

		//original Edit method - wasn't saving changes to the database (logo part was working though...)
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[CustomAuthorize(Roles = "Admin")]
		//[ValidateAntiForgeryToken]
		//public ActionResult Edit([Bind(Include = "CompanyName,CompanyLink,Address,City,State,ZipCode,Latitude,Longitude,CareerPage,Files")] Company company, HttpPostedFileBase upload)
		//{
		//          if (company == null)
		//          {
		//              return HttpNotFound();
		//          }
		//          try
		//          {
		//		Company _company = db.Companies.Find(company.CompanyName);
		//		if (upload != null && upload.ContentLength > 0)
		//		{
		//			var oldLogo = _company.Files.Any(f => f.FileType == FileType.Logo);
		//			if (oldLogo == true)
		//			{
		//				db.CompanyFiles.Remove(_company.Files.First(f => f.FileType == FileType.Logo));
		//			}
		//			var logo = new CompanyFile
		//			{
		//				FileName = System.IO.Path.GetFileName(upload.FileName),
		//				FileType = FileType.Logo,
		//				ContentType = upload.ContentType,
		//				CompanyName = company.CompanyName
		//			};
		//			using (var reader = new System.IO.BinaryReader(upload.InputStream))
		//			{
		//				logo.Content = reader.ReadBytes(upload.ContentLength);
		//			}
		//			company.Files = new List<CompanyFile> { logo };
		//		}

		//		if (ModelState.IsValid)
		//              {

		//			db.Entry(company).State = EntityState.Modified;
		//                  db.SaveChanges();
		//                  return RedirectToAction("Details", new { id = company.CompanyName });
		//              }
		//          }
		//          catch (Exception ex)
		//          {

		//              throw ex;
		//          }

		//	return View(company);
		//}


		protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
