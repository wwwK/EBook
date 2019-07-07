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
        public IHttpActionResult InsertSeller(Seller data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
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
            

            db.SaveChanges();
            

            return Ok();
        }

        public class GetRequest
        {
            public int SellerId;
        }

        [HttpGet]
        [Route("api/Seller/1")]
        public IHttpActionResult GetMerchandise(GetRequest data)
        {
            var seller = db.Sellers.Find(data.SellerId);
            if (seller == null)
            {
                return NotFound();
            }

            return Ok(seller);
        }
    }
}