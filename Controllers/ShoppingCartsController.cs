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
        private readonly OracleDbContext _db = new OracleDbContext();

//
        //insert update
        [HttpPost]
        [Route("api/ShoppingCart/")]
        public IHttpActionResult InsertShoppingCart(ShoppingCart data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return BadRequest("请先登录！");
            }
            
            int customerId = CustomerSession.GetCustomerIdFromSession(int.Parse(session.Value));
            if (customerId < 0)
            {
                return BadRequest("请先登录！");
            }
            if (_db.ShoppingCarts.Find(customerId,data.MerchandiseId) == null)
            {
                ShoppingCart shoppingCart = new ShoppingCart
                {
                    CustomerId = customerId,
                    MerchandiseId = data.MerchandiseId,
                    Amount = data.Amount,
                };


                _db.ShoppingCarts.Add(shoppingCart);
                _db.SaveChanges();

                return Ok("添加到购物车成功！");
            }

            var updateShoppingCart = _db.ShoppingCarts.FirstOrDefault(s => s.CustomerId == customerId && s.MerchandiseId == data.MerchandiseId);
            if (updateShoppingCart != null)
            {
                updateShoppingCart.Amount = data.Amount;
                _db.SaveChanges();
                return Ok("购物车更新成功！");
            }

            return BadRequest("请重新添加到购物车！");

            
        }

        public class GetRequest
        {
            public int MerchandiseId;
        }

        [HttpPost]
        [Route("api/SeeShoppingCart")]
        public IHttpActionResult SeeShoppingCart()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
 
             
            var a = new SeeShoppingCart();
            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return BadRequest("请先登录！");
            }
            
            int customerId = CustomerSession.GetCustomerIdFromSession(int.Parse(session.Value));
            if (customerId == 0)
            {
                return BadRequest("请先登录！");
            }
            return Ok(a.CheckShoppingCartWithCustomerId(customerId));
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