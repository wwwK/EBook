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
    public class TransactCartController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        [HttpPost]
        [Route("api/Transact/")]
        public async Task<IHttpActionResult> InsertTransact(Transact data)
        {
            Transact transact = new Transact
            {
                CustomerId = data.CustomerId,
                MerchandiseId = data.MerchandiseId,
                CreateTime = data.CreateTime,
                ActualPrice = data.ActualPrice,
                Status = data.Status,
                Amount = data.Amount,
                LogisticTrackNum = data.LogisticTrackNum,
                Comment = data.Comment,
            };

            db.Transacts.Add(transact);
            

            await db.SaveChangesAsync();
            

            return Ok();
        }

        public class GetRequest
        {
            public int CustomerId;
            public int MerchandiseId;
            public DateTime CreateTime;
        }

        [HttpGet]
        [Route("api/Transact/1")]
        public async Task<IHttpActionResult> GetTransact(GetRequest data)
        {
            var transact = await db.Transacts.FindAsync(data.CustomerId,data.MerchandiseId,data.CreateTime);
            if (transact == null)
            {
                throw new HttpException(404, "User not found");
            }

            return Ok(transact);
        }
    }
}