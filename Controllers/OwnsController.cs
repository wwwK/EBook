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
    public class OwnController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        //insert update
        [HttpPost]
        [Route("api/Own/")]
        public IHttpActionResult InsertOwn(Own data)
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

            int customerId = CustomerSession.GetCustomerIdFromSession(int.Parse(session.Value));
            if (customerId < 0)
            {
                return BadRequest("请先登录！");
            }

            if (db.Owns.Find(customerId,data.CouponId) == null)
            {
                Own own = new Own
                {
                    CustomerId = customerId,
                    CouponId = data.CouponId,
                };


                db.Owns.Add(own);
                db.SaveChanges();

                return Ok("获取优惠券成功！");
            }

            
            
            var updateOwn = db.Owns.FirstOrDefault(o => o.CustomerId == customerId && o.CouponId == data.CouponId);
            if (updateOwn != null)
            {
                updateOwn.IsValid = data.IsValid;
                db.SaveChanges();
                return Ok("使用成功！");
            }

            return BadRequest("请重新操作优惠券！");
        }


    }
}