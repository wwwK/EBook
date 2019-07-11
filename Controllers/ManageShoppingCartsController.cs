using System.Web.Http;
using EBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using BCrypt.Net;
using System.Web.SessionState;
using NETCore.Encrypt;
using EBook.Service;


namespace EBook.Controllers
{
    public class ManageShoppingCartsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [HttpPost]
        [Route("api/ManageSelectShoppingCart")]
        public IHttpActionResult ManageSelectShoppingCart()
        {
            ShoppingCart[] tableShoppingCarts = db.ShoppingCarts.ToArray();
            IEnumerable<ShoppingCart> selectTableShoppingCarts =
                from shoppingCart in tableShoppingCarts
                select shoppingCart;
            return Ok(selectTableShoppingCarts.ToArray());
        }


        [HttpPost]
        [Route("api/ManageInsertShoppingCart")]
        public IHttpActionResult ManageInsertShoppingCart(ShoppingCart shoppingCart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ShoppingCarts.Add(shoppingCart);
            db.SaveChanges();

            return Ok("Insert Success");
        }
        
        [HttpPost]
        [Route("api/ManageUpdateShoppingCart")]
        public IHttpActionResult ManageUpdateShoppingCart(ShoppingCart shoppingCart)
        {
            ShoppingCart updatedShoppingCart = db.ShoppingCarts.FirstOrDefault(s => s.CustomerId == shoppingCart.CustomerId && s.MerchandiseId == shoppingCart.MerchandiseId);
            if (updatedShoppingCart != null)
            {
                updatedShoppingCart.IsValid = 0;
                db.SaveChanges();
                return Ok("更新成功！");
            }

            return BadRequest("请重新操作！");
        }
    }
}