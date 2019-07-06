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
    public class MerchandiseController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        [HttpPost]
        [Route("api/Merchandise/")]
        public async Task<IHttpActionResult> InsertMerchandise(Merchandise data)
        {
            Merchandise merchandise = new Merchandise
            {
                MerchandiseId = data.MerchandiseId,
                SellerId = data.SellerId,
                ISBN = data.ISBN,
                Description = data.Description,
                Price = data.Price,
            };

            db.Merchandises.Add(merchandise);
            

            await db.SaveChangesAsync();
            

            return Ok();
        }

        public class GetRequest
        {
            public int MerchandiseId;
        }

        [HttpGet]
        [Route("api/Merchandise/1")]
        public async Task<IHttpActionResult> GetMerchandise(GetRequest data)
        {
            var merchandise = await db.Merchandises.FindAsync(data.MerchandiseId);
            if (merchandise == null)
            {
                throw new HttpException(404, "User not found");
            }

            return Ok(merchandise);
        }
    }
}