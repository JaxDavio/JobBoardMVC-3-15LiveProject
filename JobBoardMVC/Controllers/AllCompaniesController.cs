using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobBoardMVC.Models;

namespace JobBoardMVC.Controllers
{
    public class AllCompaniesController : Controller
    {
        private JobBoardMvcContext db = new JobBoardMvcContext();

        // GET: AllCompanies
        public ActionResult Index()
        {
            var allCompanies = db.Companies;

            // grab a count of all companies in the database
            int count = allCompanies.Count();
            // store it in viewbag for the View to display
            ViewBag.Counts = count;

            return View(allCompanies.ToList());
        }
    }
}
