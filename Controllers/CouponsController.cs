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
        public async Task<IHttpActionResult> InsertCoupon(Coupon data)
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


            await db.SaveChangesAsync();


            return Ok();
        }

        public class GetRequest
        {
            public int CouponId;
        }

        [HttpGet]
        [Route("api/Coupon/1")]
        public async Task<IHttpActionResult> GetCoupon(GetRequest data)
        {
            var coupon = await db.Coupons.FindAsync(data.CouponId);
            if (coupon == null)
            {
                throw new HttpException(404, "User not found");
            }

            return Ok(coupon);
        }
    }
}