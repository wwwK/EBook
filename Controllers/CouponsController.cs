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
    public class CouponsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/Coupons
        public IQueryable<Coupon> GetCoupons()
        {
            return db.Coupons;
        }

        // GET: api/Coupons/5
        [ResponseType(typeof(Coupon))]
        public async Task<IHttpActionResult> GetCoupon(int id)
        {
            Coupon coupon = await db.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }

            return Ok(coupon);
        }

        // PUT: api/Coupons/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCoupon(int id, Coupon coupon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != coupon.CouponId)
            {
                return BadRequest();
            }

            db.Entry(coupon).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CouponExists(id))
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

        // POST: api/Coupons
        [ResponseType(typeof(Coupon))]
        public async Task<IHttpActionResult> PostCoupon(Coupon coupon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Coupons.Add(coupon);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = coupon.CouponId }, coupon);
        }

        // DELETE: api/Coupons/5
        [ResponseType(typeof(Coupon))]
        public async Task<IHttpActionResult> DeleteCoupon(int id)
        {
            Coupon coupon = await db.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }

            db.Coupons.Remove(coupon);
            await db.SaveChangesAsync();

            return Ok(coupon);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CouponExists(int id)
        {
            return db.Coupons.Count(e => e.CouponId == id) > 0;
        }
    }
}