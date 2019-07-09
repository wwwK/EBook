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
using EBook.Service;

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

            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return BadRequest("Not Login");
            }

            var sellerId = SellerSession.GetSellerIdFromSession(int.Parse(session.Value));
            if (sellerId < 0)
            {
                return BadRequest("Not Login");
            }

            if (db.Coupons.Find(data.CouponId) == null)
            {
                Coupon coupon = new Coupon
                {
                    CouponId = data.CouponId,
                    ReleaseBySellerId = sellerId,
                    DiscountAmount = data.DiscountAmount,
                    ValidThrough = data.ValidThrough,
                    PriceLimit = data.PriceLimit,
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
                db.SaveChanges();
                return Ok("Update Success");
            }

            return BadRequest("Unable to Insert and Update");
        }

        public class GetRequest
        {
            public int CouponId;
        }

        public class CouponInfo
        {
            public int CouponId;

            public int DiscountAmount;

            public DateTime ValidThrough;

            public int PriceLimit;
        }

        //get ok
        [HttpPost]
        [Route("api/GetCoupon")]
        public IHttpActionResult GetCoupon(GetRequest data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var coupon = db.Coupons.Find(data.CouponId);
            if (coupon == null || coupon.IsValid == 0)
            {
                return NotFound();
            }


            return Ok(coupon);
        }
    }
}