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
        private readonly OracleDbContext _db = new OracleDbContext();


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
                return BadRequest("请先登录！");
            }

            var sellerId = SellerSession.GetSellerIdFromSession(int.Parse(session.Value));
            if (sellerId < 0)
            {
                return BadRequest("请先登录！");
            }

            if (_db.Coupons.Find(data.CouponId) == null)
            {
                Coupon coupon = new Coupon
                {
                    ReleaseBySellerId = sellerId,
                    DiscountAmount = data.DiscountAmount,
                    ValidThrough = data.ValidThrough,
                    PriceLimit = data.PriceLimit,
                };

                _db.Coupons.Add(coupon);
                _db.SaveChanges();

                return Ok("优惠券发放成功！");
            }

            var updatecoupon = _db.Coupons.FirstOrDefault(c => c.CouponId == data.CouponId);
            if (updatecoupon != null)
            {
                updatecoupon.DiscountAmount = data.DiscountAmount;
                updatecoupon.ValidThrough = data.ValidThrough;
                updatecoupon.PriceLimit = data.PriceLimit;
                _db.SaveChanges();
                return Ok("优惠规则更新成功！");
            }

            return BadRequest("请重新设置优惠券");
        }

        public class GetRequest
        {
            public readonly int CouponId;

            public GetRequest(int couponId)
            {
                CouponId = couponId;
            }
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

            var coupon = _db.Coupons.Find(data.CouponId);
            if (coupon == null || coupon.IsValid == 0)
            {
                return NotFound();
            }


            return Ok(coupon);
        }
    }
}