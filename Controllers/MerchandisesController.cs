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
    public class MerchandisesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/Merchandises
        public IQueryable<Merchandise> GetMerchandises()
        {
            return db.Merchandises;
        }

        // GET: api/Merchandises/5
        [ResponseType(typeof(Merchandise))]
        public async Task<IHttpActionResult> GetMerchandise(int id)
        {
            Merchandise merchandise = await db.Merchandises.FindAsync(id);
            if (merchandise == null)
            {
                return NotFound();
            }

            return Ok(merchandise);
        }

        // PUT: api/Merchandises/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMerchandise(int id, Merchandise merchandise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != merchandise.MerchandiseId)
            {
                return BadRequest();
            }

            db.Entry(merchandise).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MerchandiseExists(id))
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

        // POST: api/Merchandises
        [ResponseType(typeof(Merchandise))]
        public async Task<IHttpActionResult> PostMerchandise(Merchandise merchandise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Merchandises.Add(merchandise);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = merchandise.MerchandiseId }, merchandise);
        }

        // DELETE: api/Merchandises/5
        [ResponseType(typeof(Merchandise))]
        public async Task<IHttpActionResult> DeleteMerchandise(int id)
        {
            Merchandise merchandise = await db.Merchandises.FindAsync(id);
            if (merchandise == null)
            {
                return NotFound();
            }

            db.Merchandises.Remove(merchandise);
            await db.SaveChangesAsync();

            return Ok(merchandise);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MerchandiseExists(int id)
        {
            return db.Merchandises.Count(e => e.MerchandiseId == id) > 0;
        }
    }
}