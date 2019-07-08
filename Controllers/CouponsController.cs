﻿using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EBook.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using BCrypt.Net;
using System.Web.SessionState;


namespace EBook.Controllers
{
    public class CouponController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        //insert update
        [HttpPost]
        [Route("api/Coupon/")]
        public IHttpActionResult InsertCoupon(Coupon data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (db.Coupons.Find(data.CouponId) == null)
            {
                Coupon coupon = new Coupon
                {
                    CouponId = data.CouponId,
                    ReleaseBySellerId = data.ReleaseBySellerId,
                    DiscountAmount = data.DiscountAmount,
                    ValidThrough = data.ValidThrough,
                    PriceLimit = data.PriceLimit,
                    CouponStatus =data.CouponStatus,
                };
                
                db.Coupons.Add(coupon);
                db.SaveChanges();

                return Ok("Insert Success");
            }

            var updatecoupon = db.Coupons.FirstOrDefault(c => c.CouponId == data.CouponId);
            if (updatecoupon != null)
            {
                updatecoupon.DiscountAmount = data.DiscountAmount;
                updatecoupon.ValidThrough = data.ValidThrough;
                updatecoupon.PriceLimit = data.PriceLimit;
                updatecoupon.CouponStatus = data.CouponStatus;
                db.SaveChanges();
                return Ok("Update Success");
            }

            return BadRequest("Unable to Insert and Update");
        }

        public class GetRequest
        {
            public int CouponId;
        }

        [HttpGet]
        [Route("api/Coupon/{CouponId}")]
        public IHttpActionResult GetCoupon(int CouponId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var coupon = db.Coupons.Find(CouponId);
            if (coupon == null)
            {
                return NotFound();
            }

            return Ok(coupon);
        }
    }
}