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

            if (db.Merchandises.Find(data.MerchandiseId) == null)
            {
                Merchandise merchandise = new Merchandise
                {
                    MerchandiseId = data.MerchandiseId,
                    SellerId = data.SellerId,
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

        


        [HttpGet]
        [Route("api/Merchandise/{MerchandiseId}")]
        public IHttpActionResult GetMerchandise(int MerchandiseId)
        {
            var merchandise = db.Merchandises.Find(MerchandiseId);
            if (merchandise == null)
            {
                return NotFound();
            }

            return Ok(merchandise);
        }
    }
}