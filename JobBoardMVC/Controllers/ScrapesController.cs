using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using JobBoardMVC.Models;
using System.Data.SqlClient;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Data.Entity;


namespace JobBoardMVC.Controllers
{
    public class ScrapesController : Controller
    {
        private JobBoardMvcContext db = new JobBoardMvcContext();

        public ActionResult Index()
        {
            return View(db);
        }

        public ActionResult Scrape(string id)
        {
            var model = new CompanyViewModel();
            
            model.company = db.Companies.Include(c => c.Files).SingleOrDefault(c => c.CompanyName == id);

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(model.company.CareerPage);

            IEnumerable<HtmlNode> jobTitleAndLinkArray = doc.DocumentNode.CssSelect(model.company.JobCode);

            List < Job> jobs = new List<Job>();

            for (int i = 0; i < jobTitleAndLinkArray.Count(); i++)
            {
                Job job = new Job();

                HtmlNode titleAndLink = jobTitleAndLinkArray.ElementAt(i);             

                job.ApplicationLink = model.company.CareerPage + titleAndLink.ChildNodes[1].Attributes["href"].Value;
                job.CompanyCompanyName = model.company.CompanyName;
                job.DatePosted = DateTime.Now.ToString();
                job.Experience = "";
                job.Hours = "";
                job.JobID = "";
                job.JobTitle = System.Net.WebUtility.HtmlDecode(titleAndLink.ChildNodes[1].InnerText);
                job.LanguagesUsed = "";
                job.Location = model.company.City;
                job.Salary = "";

                jobs.Add(job);


            }

            UploadScrapes(jobs, model.company.CompanyName);

            return View(db);
        }

        public static void UploadScrapes(List<Job> jobs, string company)
        {

            try
            {
                // These are the credentials to the DB and Table where scraped data will go.
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder("Data Source=(localdb)\\ProjectsV13;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    sb.Append("DELETE FROM [dbo].[JobTable] WHERE [CompanyCompanyName] = @CompanyCompanyName;");
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CompanyCompanyName", company);
                        command.ExecuteNonQuery();
                    }
                    sb.Clear();

                    Random rnd = new Random();


                    // This uploads all job postings from the target company from the current scrape.
                    foreach (var job in jobs)
                    {
                        sb.Append("INSERT INTO [dbo].[JobTable] ([Id], [ApplicationLink], [CompanyCompanyName], [DatePosted], [Experience], [Hours], [JobID], [JobTitle], [LanguagesUsed], [Location], [Salary])");
                        sb.Append("VALUES (@Id, @ApplicationLink, @CompanyCompanyName, @DatePosted, @Experience, @Hours, @JobID, @JobTitle, @LanguagesUsed, @Location, @Salary);");

                        int random = rnd.Next(1, 100000);

                        sql = sb.ToString();
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Id", random);
                            command.Parameters.AddWithValue("@ApplicationLink", job.ApplicationLink);
                            command.Parameters.AddWithValue("@CompanyCompanyName", job.CompanyCompanyName);
                            command.Parameters.AddWithValue("@DatePosted", job.DatePosted);
                            command.Parameters.AddWithValue("@Experience", job.Experience);
                            command.Parameters.AddWithValue("@Hours", job.Hours);
                            command.Parameters.AddWithValue("@JobID", job.JobID);
                            command.Parameters.AddWithValue("@JobTitle", job.JobTitle);
                            command.Parameters.AddWithValue("@LanguagesUsed", job.LanguagesUsed);
                            command.Parameters.AddWithValue("@Location", job.Location);
                            command.Parameters.AddWithValue("@Salary", job.Salary);
                            command.ExecuteNonQuery();
                        }
                        sb.Clear();
                    }

                    try
                    {
                        connection.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }

                    // This is just a confirmation that the job was uploaded with a timestamp.
                    Console.Write(String.Format("Jobs from {0} last uploaded on {1}.\n\n", company, DateTime.Now));
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}
