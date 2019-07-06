using System.Threading.Tasks;
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


        [HttpPost]
        [Route("api/Coupon/")]
        public IHttpActionResult InsertCoupon(Coupon data)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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
            
            return Ok();
        }

        public class GetRequest
        {
            public int CouponId;
        }

        [HttpGet]
        [Route("api/Coupon/1")]
        public IHttpActionResult GetCoupon(GetRequest data)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var coupon = db.Coupons.Find(data.CouponId);
            if (coupon == null)
            {
                return NotFound();
            }

            return Ok(coupon);
        }
    }
}