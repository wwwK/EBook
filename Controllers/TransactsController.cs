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
    public class TransactsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/Transacts
        public IQueryable<Transact> GetTransacts()
        {
            return db.Transacts;
        }

        // GET: api/Transacts/5
        [ResponseType(typeof(Transact))]
        public async Task<IHttpActionResult> GetTransact(int id)
        {
            Transact transact = await db.Transacts.FindAsync(id);
            if (transact == null)
            {
                return NotFound();
            }

            return Ok(transact);
        }

        // PUT: api/Transacts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTransact(int id, Transact transact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transact.CustomerId)
            {
                return BadRequest();
            }

            db.Entry(transact).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactExists(id))
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

        // POST: api/Transacts
        [ResponseType(typeof(Transact))]
        public async Task<IHttpActionResult> PostTransact(Transact transact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Transacts.Add(transact);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TransactExists(transact.CustomerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = transact.CustomerId }, transact);
        }

        // DELETE: api/Transacts/5
        [ResponseType(typeof(Transact))]
        public async Task<IHttpActionResult> DeleteTransact(int id)
        {
            Transact transact = await db.Transacts.FindAsync(id);
            if (transact == null)
            {
                return NotFound();
            }

            db.Transacts.Remove(transact);
            await db.SaveChangesAsync();

            return Ok(transact);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactExists(int id)
        {
            return db.Transacts.Count(e => e.CustomerId == id) > 0;
        }
    }
}