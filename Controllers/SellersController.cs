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
    public class SellerController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        [HttpPost]
        [Route("api/Seller/")]
        public async Task<IHttpActionResult> InsertSeller(Seller data)
        {
            Seller seller = new Seller
            {
                SellerId = data.SellerId,
                Password = data.Password,
                ShopName = data.ShopName,
                CreditLevel = data.CreditLevel,
                ShopDescription = data.ShopDescription,
                City = data.City,
                SellerPhone = data.SellerPhone,

            };

            db.Sellers.Add(seller);
            

            await db.SaveChangesAsync();
            

            return Ok();
        }

        public class GetRequest
        {
            public int SellerId;
        }

        [HttpGet]
        [Route("api/Seller/1")]
        public async Task<IHttpActionResult> GetMerchandise(GetRequest data)
        {
            var seller = await db.Sellers.FindAsync(data.SellerId);
            if (seller == null)
            {
                throw new HttpException(404, "User not found");
            }

            return Ok(seller);
        }
    }
}