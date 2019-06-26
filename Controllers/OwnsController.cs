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
    public class OwnsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/Owns
        public IQueryable<Own> GetOwns()
        {
            return db.Owns;
        }

        // GET: api/Owns/5
        [ResponseType(typeof(Own))]
        public async Task<IHttpActionResult> GetOwn(int id)
        {
            Own own = await db.Owns.FindAsync(id);
            if (own == null)
            {
                return NotFound();
            }

            return Ok(own);
        }

        // PUT: api/Owns/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOwn(int id, Own own)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != own.CustomerId)
            {
                return BadRequest();
            }

            db.Entry(own).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OwnExists(id))
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

        // POST: api/Owns
        [ResponseType(typeof(Own))]
        public async Task<IHttpActionResult> PostOwn(Own own)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Owns.Add(own);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OwnExists(own.CustomerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = own.CustomerId }, own);
        }

        // DELETE: api/Owns/5
        [ResponseType(typeof(Own))]
        public async Task<IHttpActionResult> DeleteOwn(int id)
        {
            Own own = await db.Owns.FindAsync(id);
            if (own == null)
            {
                return NotFound();
            }

            db.Owns.Remove(own);
            await db.SaveChangesAsync();

            return Ok(own);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OwnExists(int id)
        {
            return db.Owns.Count(e => e.CustomerId == id) > 0;
        }
    }
}