using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using EBook.Models;

namespace EBook.Controllers
{
    public class ShoppingCartsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/ShoppingCarts
        public IQueryable<ShoppingCart> GetShoppingCarts()
        {
            return db.ShoppingCarts;
        }

        // GET: api/ShoppingCarts/5
        [ResponseType(typeof(ShoppingCart))]
        public async Task<IHttpActionResult> GetShoppingCart(int id)
        {
            ShoppingCart shoppingCart = await db.ShoppingCarts.FindAsync(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return Ok(shoppingCart);
        }

        // PUT: api/ShoppingCarts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutShoppingCart(int id, ShoppingCart shoppingCart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shoppingCart.CustomerId)
            {
                return BadRequest();
            }

            db.Entry(shoppingCart).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoppingCartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ShoppingCarts
        [ResponseType(typeof(ShoppingCart))]
        public async Task<IHttpActionResult> PostShoppingCart(ShoppingCart shoppingCart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ShoppingCarts.Add(shoppingCart);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ShoppingCartExists(shoppingCart.CustomerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = shoppingCart.CustomerId }, shoppingCart);
        }

        // DELETE: api/ShoppingCarts/5
        [ResponseType(typeof(ShoppingCart))]
        public async Task<IHttpActionResult> DeleteShoppingCart(int id)
        {
            ShoppingCart shoppingCart = await db.ShoppingCarts.FindAsync(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            db.ShoppingCarts.Remove(shoppingCart);
            await db.SaveChangesAsync();

            return Ok(shoppingCart);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ShoppingCartExists(int id)
        {
            return db.ShoppingCarts.Count(e => e.CustomerId == id) > 0;
        }
    }
}