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
        public async Task<IHttpActionResult> InsertCollect(Collect data)
        {
            Collect collect = new Collect
            {
                CustomerId = data.CustomerId,
                MerchandiseId = data.MerchandiseId,
                CollectTime = data.CollectTime
            };


            db.Collects.Add(collect);
            

            await db.SaveChangesAsync();
            

            return Ok();
        }


        public class GetRequest
        {
            public int CustomerId;
            public int MerchandiseId;
        }

        [HttpGet]
        [Route("api/Collect/1")]
        public async Task<IHttpActionResult> GetCollect(GetRequest data)
        {
            var collect = await db.Collects.FindAsync(data.CustomerId,data.MerchandiseId);
            if (collect == null)
            {
                throw new HttpException(404, "User not found");
            }

            return Ok(collect);
        }
    }
}