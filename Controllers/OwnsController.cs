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
                return BadRequest("Not Login");
            }

            int customerId = CustomerSession.GetCustomerIdFromSession(int.Parse(session.Value));
            if (customerId < 0)
            {
                return BadRequest("Not Login");
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

                return Ok("Insert Success");
            }

            
            
            var updateown = db.Owns.FirstOrDefault(o => o.CustomerId == customerId && o.CouponId == data.CouponId);
            if (updateown != null)
            {
                updateown.IsValid = data.IsValid;
                db.SaveChanges();
                return Ok("Update Success");
            }

            return BadRequest("Unable to Insert and Update");
        }

       /* public class GetRequest
        {
            public int CustomerId;
            public int CouponId;
        }

        [HttpGet]
        [Route("api/Own/1")]
        public IHttpActionResult GetOwn(GetRequest data)
        {
            var own = db.Owns.Find(data.CustomerId,data.CouponId);
            if (own == null)
            {
                return NotFound();
            }

            return Ok(own);
        }*/
    }
}