using System.Web.Http;
using EBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using BCrypt.Net;
using System.Web.SessionState;
using NETCore.Encrypt;
using EBook.Service;


namespace EBook.Controllers
{
    public class ManageCouponsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [HttpPost]
        [Route("api/ManageSelectCoupon")]
        public IHttpActionResult ManageSelectCoupon()
        {
            Coupon [] tableCoupons = db.Coupons.ToArray();
            IEnumerable<Coupon>selectTableCoupons =
                from coupon in tableCoupons
                select coupon;
            return Ok(selectTableCoupons.ToArray());

        }
        
        
        [HttpPost]
        [Route("api/ManageInsertCoupon")]
        public IHttpActionResult ManageInsertCoupon(Coupon coupon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Coupons.Add(coupon);
            db.SaveChanges();

            return Ok("Insert Success");
        }
        
        [HttpPost]
        [Route("api/ManageUpdateCoupon")]
        public IHttpActionResult ManageUpdateCoupon(Coupon coupon)
        {
            Coupon updatedCoupon = db.Coupons.FirstOrDefault(c => c.CouponId == coupon.CouponId);
            if (updatedCoupon != null)
            {
                updatedCoupon = coupon;
                db.SaveChanges();
                return Ok("更新成功！");
            }

            return BadRequest("请重新操作！");
        }
        
    }

}