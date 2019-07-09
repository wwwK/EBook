using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EBook.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using BCrypt.Net;
using System.Web.SessionState;
using EBook.Service;


namespace EBook.Controllers
{
    public class MerchandiseController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        
        //insert update
        [HttpPost]
        [Route("api/Merchandise/")]
        public IHttpActionResult InsertMerchandise(Merchandise data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return BadRequest("Not Login");
            }

            int sellseId = SellerSession.GetSellerIdFromSession(int.Parse(session.Value));
            if (sellseId < 0)
            {
                return BadRequest("Not Login");
            }
            if (db.Merchandises.Find(data.MerchandiseId) == null)
            {
                Merchandise merchandise = new Merchandise
                {
                    MerchandiseId = data.MerchandiseId,
                    SellerId = sellseId,        
                    ISBN = data.ISBN,
                    Description = data.Description,
                    Price = data.Price,
                    IsValid = data.IsValid,
                };


                db.Merchandises.Add(merchandise);
                db.SaveChanges();

                return Ok("Insert Success");
            }

            var updatedmerchandise = db.Merchandises.FirstOrDefault(m => m.MerchandiseId == data.MerchandiseId);
            if (updatedmerchandise != null)
            {
                updatedmerchandise.ISBN = data.ISBN;
                updatedmerchandise.Description = data.Description;
                updatedmerchandise.Price = data.Price;
                updatedmerchandise.IsValid = data.IsValid;
                db.SaveChanges();
                return Ok("Update Success");
            }

            return BadRequest("Unable to Insert and Update");


        }

        public class GetRequest
        {
            public int MerchandiseId;
            public string Comment;
        }


        [HttpPost]
        [Route("api/GetMerchandise")]
        public IHttpActionResult GetMerchandise(GetRequest data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
 

            BookInfo[] books = BookSearch.BookSearchWithMerchandiseId(data.MerchandiseId);
            if (books.Length == 0)
            {
                return BadRequest("No Merchandise Found");
            }
            return Ok(books[0]);
        }
//get ok
/*
 ++       
        [HttpGet]
        [Route("api/GetMerchandise")]
        public IHttpActionResult GetMerchandise(GetRequest data)
        {
            var merchandise = db.Merchandises.Find(data.MerchandiseId);
            if (merchandise == null)
            {
                return NotFound();
            }

            return Ok(merchandise);
        }*/

        public class SellerRequest
        {
            public string sellerShopName;
        }
        [HttpPost]
        [Route("api/GetMerchandisesOfSeller")]
        public IHttpActionResult GetMerchandisesOfSeller(SellerRequest data)
        {
            var result =
                (from seller in db.Sellers
                    where seller.ShopName == data.sellerShopName
                    join merchandise in db.Merchandises on seller.SellerId equals merchandise.SellerId
                 select merchandise).ToArray();

            if (result.Length == 0)
            {
                return NotFound();
            }

            return Ok(result);

        }
        
        
        
        
        
    }
}