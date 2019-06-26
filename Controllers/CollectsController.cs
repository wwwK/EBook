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
using EBook.Models;

namespace EBook.Controllers
{
    public class CollectsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/Collects
        public IQueryable<Collect> GetCollects()
        {
            return db.Collects;
        }

        // GET: api/Collects/5
        [ResponseType(typeof(Collect))]
        public async Task<IHttpActionResult> GetCollect(int id)
        {
            Collect collect = await db.Collects.FindAsync(id);
            if (collect == null)
            {
                return NotFound();
            }

            return Ok(collect);
        }

        // PUT: api/Collects/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCollect(int id, Collect collect)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != collect.CustomerId)
            {
                return BadRequest();
            }

            db.Entry(collect).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CollectExists(id))
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

        // POST: api/Collects
        [ResponseType(typeof(Collect))]
        public async Task<IHttpActionResult> PostCollect(Collect collect)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Collects.Add(collect);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CollectExists(collect.CustomerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = collect.CustomerId }, collect);
        }

        // DELETE: api/Collects/5
        [ResponseType(typeof(Collect))]
        public async Task<IHttpActionResult> DeleteCollect(int id)
        {
            Collect collect = await db.Collects.FindAsync(id);
            if (collect == null)
            {
                return NotFound();
            }

            db.Collects.Remove(collect);
            await db.SaveChangesAsync();

            return Ok(collect);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CollectExists(int id)
        {
            return db.Collects.Count(e => e.CustomerId == id) > 0;
        }
    }
}