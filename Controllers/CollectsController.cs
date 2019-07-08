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
    public class CollectController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        //insert update
        [HttpPost]
        [Route("api/Collect/")]
        public IHttpActionResult InsertCollect(Collect data)
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
            if (db.Collects.Find(data.CustomerId,data.MerchandiseId) == null)
            {
                Collect collect = new Collect
                {
                    CustomerId = customerId,
                    MerchandiseId = data.MerchandiseId,
                    CollectTime = data.CollectTime,
                };
                
                db.Collects.Add(collect);
                db.SaveChanges();

                return Ok("Insert Success");
            }

            var updatecollect = db.Collects.FirstOrDefault(c => c.CustomerId == customerId && c.MerchandiseId == data.MerchandiseId);
            if (updatecollect != null)
            {
                updatecollect.CollectTime = data.CollectTime;
                updatecollect.IsValid = data.IsValid;
                db.SaveChanges();
                return Ok("Update Success");
            }

            return BadRequest("Unable to Insert and Update");

        }

/*
        public class GetRequest
        {
            public int CustomerId;
            public int MerchandiseId;
        }

        [HttpGet]
        [Route("api/Collect/1")]
        public IHttpActionResult GetCollect(GetRequest data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var collect = db.Collects.Find(data.CustomerId,data.MerchandiseId);
            if (collect == null)
            {
                return NotFound();
            }

            return Ok(collect);
        }*/
    }
}