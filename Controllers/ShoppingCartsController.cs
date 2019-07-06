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
    public class ShoppingCartController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        [HttpPost]
        [Route("api/ShoppingCart/")]
        public async Task<IHttpActionResult> InsertShoppingCart(ShoppingCart data)
        {
            ShoppingCart shoppingCart = new ShoppingCart
            {
                CustomerId = data.CustomerId,
                MerchandiseId = data.MerchandiseId,
                Amount = data.Amount,

            };

            db.ShoppingCarts.Add(shoppingCart);
            

            await db.SaveChangesAsync();
            

            return Ok();
        }

        public class GetRequest
        {
            public int CustomerId;
            public int MerchandiseId;
        }

        [HttpGet]
        [Route("api/ShoppingCart/1")]
        public async Task<IHttpActionResult> GetShoppingCart(GetRequest data)
        {
            var shoppingCart = await db.ShoppingCarts.FindAsync(data.CustomerId,data.MerchandiseId);
            if (shoppingCart == null)
            {
                throw new HttpException(404, "User not found");
            }

            return Ok(shoppingCart);
        }
    }
}