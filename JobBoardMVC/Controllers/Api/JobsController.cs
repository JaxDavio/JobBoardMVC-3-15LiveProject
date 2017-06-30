using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using JobBoardMVC.Models;
using System.Data.SqlClient;

namespace JobBoardMVC.Controllers.Api
{
    public class JobsController : ApiController
    {
        private JobBoardMvcContext db = new JobBoardMvcContext();


        /* broken at 5:30 4/19/17 
         * cause trying to display company column with non null value
         */
        // GET: api/Jobs
        // Return: json of all jobs in table
        public IQueryable<Job> GetJobs()
        {
            return db.Jobs;
        }
        
        [Route("api/jobs/full")]
        [ResponseType(typeof(Job))]
        public IQueryable<Job> GetJobscomp()
        {
            return db.Jobs.Include(c => c.Company);
        }

        [Route("api/jobs/count")]
        public int GetCount()
        {
            var count = db.Jobs.Count();
            return count;
        }

        [Route("api/jobs/name/{title}")]
        [ResponseType(typeof(Job))]
        public IQueryable<Job> GetJobName(string title)
        {
            var queryExample = from c in db.Jobs where c.JobTitle.Contains(title) select c;

            return queryExample;

        }

        [Route("api/jobs/name/{title}/full")]
        [ResponseType(typeof(Job))]
        public IQueryable<Job> GetJobNameFull(string title)
        {
            var queryExample = from c in db.Jobs where c.JobTitle.Contains(title) select c;

            return queryExample.Include(c => c.Company);

        }

        [Route("api/jobs/name/{title}/count")]
        public int GetJobNameCount(string title)
        {
            var test = db.Jobs.Where(j => title.Any(check => j.JobTitle.Contains(check)));
            var query = from c in db.Jobs where c.JobTitle.Contains(title) select c;
            int count = query.Count();
            return count;

        }


        [Route("api/jobs/location/{place}")]
        [ResponseType(typeof(Job))]
        public IQueryable<Job> GetJobLoc(string place)
        {
            var queryExample = from c in db.Jobs where c.Location.Contains(place) select c;
            return queryExample;

        }

        [Route("api/jobs/location/{place}/full")]
        [ResponseType(typeof(Job))]
        public IQueryable<Job> GetJobLocFull(string place)
        {
            var queryExample = from c in db.Jobs where c.Location.Contains(place) select c;
            return queryExample.Include(c => c.Company);

        }

        [Route("api/jobs/location/{place}/count")]
        public int GetJobLocCount(string place)
        {
            var test = db.Jobs.Where(j => place.Any(check => j.JobTitle.Contains(check)));
            var query = from c in db.Jobs where c.JobTitle.Contains(place) select c;
            int count = query.Count();
            return count;

        }

        [Route("api/jobs/comp/{name}")]
        [ResponseType(typeof(Job))]
        public IQueryable<Job> GetJobComp(string name)
        {
            var queryExample = from c in db.Jobs where c.CompanyCompanyName.Contains(name) select c;
            return queryExample;

        }

        [Route("api/jobs/comp/{name}/full")]
        [ResponseType(typeof(Job))]
        public IQueryable<Job> GetJobCompFull(string name)
        {
            var queryExample = from c in db.Jobs where c.CompanyCompanyName.Contains(name) select c;
            return queryExample.Include(c => c.Company);

        }

        [Route("api/jobs/comp/{name}/count")]
        public int GetJobCompCount(string name)
        {
            var test = db.Jobs.Where(j => name.Any(check => j.JobTitle.Contains(check)));
            var query = from c in db.Jobs where c.JobTitle.Contains(name) select c;
            int count = query.Count();
            return count;

        }

        /*
        // GET: api/Jobs/5
        [ResponseType(typeof(Job))]
        public async Task<IHttpActionResult> GetJob(int id)
        {
            Job job = await db.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            return Ok(job);
        }
        
        // PUT: api/Jobs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutJob(int id, Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != job.ID)
            {
                return BadRequest();
            }

            db.Entry(job).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Jobs
        [ResponseType(typeof(Job))]
        public async Task<IHttpActionResult> PostJob(Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Jobs.Add(job);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = job.ID }, job);
        }

        // DELETE: api/Jobs/5
        [ResponseType(typeof(Job))]
        public async Task<IHttpActionResult> DeleteJob(int id)
        {
            Job job = await db.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            db.Jobs.Remove(job);
            await db.SaveChangesAsync();

            return Ok(job);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JobExists(int id)
        {
            return db.Jobs.Count(e => e.ID == id) > 0;
        }
        */
    }
}