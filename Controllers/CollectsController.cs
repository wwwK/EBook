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
    public class CollectController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        [HttpPost]
        [Route("api/Collect/")]
        public IHttpActionResult InsertCollect(Collect data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Collect collect = new Collect
            {
                CustomerId = data.CustomerId,
                MerchandiseId = data.MerchandiseId,
                CollectTime = data.CollectTime
            };
            
            db.Collects.Add(collect);

            db.SaveChanges();
            
            return Ok();
        }


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
        }
    }
}