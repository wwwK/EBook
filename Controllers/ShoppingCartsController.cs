using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EBook.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Results;
using BCrypt.Net;
using System.Web.SessionState;
using EBook.Service;


namespace EBook.Controllers
{
    public class ShoppingCartController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        [HttpPost]
        [Route("api/ShoppingCart/")]
        public IHttpActionResult InsertShoppingCart(ShoppingCart data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            ShoppingCart shoppingCart = new ShoppingCart
            {
                CustomerId = data.CustomerId,
                MerchandiseId = data.MerchandiseId,
                Amount = data.Amount,

            };

            db.ShoppingCarts.Add(shoppingCart);

            db.SaveChanges();
            

            return Ok();
        }

        public class GetRequest
        {
            public int MerchandiseId;
        }

        [HttpPost]
        [Route("api/SeeShoppingCart")]
        public IHttpActionResult SeeShoppingCart(GetRequest data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
 
             
            var a = new SeeShoppingCart();
            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return BadRequest("Not Login");
            }
            
            int customerId = CustomerSession.GetCustomerIdFromSession(int.Parse(session.Value));
            if (customerId == 0)
            {
                return BadRequest("Not Login");
            }
            return Ok(a.SeeShoppingCartWithCustomerId(customerId));
        }

        /*[HttpGet]
        [Route("api/ShoppingCart/1")]
        public IHttpActionResult GetShoppingCart(GetRequest data)
        {
            var shoppingCart = db.ShoppingCarts.Find(data.CustomerId,data.MerchandiseId);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return Ok(shoppingCart);
        }*/
    }
}