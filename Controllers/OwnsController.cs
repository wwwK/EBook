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
    public class OwnController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        [HttpPost]
        [Route("api/Own/")]
        public IHttpActionResult InsertOwn(Own data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            Own own = new Own
            {
                CustomerId = data.CustomerId,
                CouponId = data.CouponId,
                Status = data.Status,

            };

            db.Owns.Add(own);
            

            db.SaveChanges();
            

            return Ok();
        }

        public class GetRequest
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
        }
    }
}