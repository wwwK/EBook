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
    public class BookController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        [HttpPost]
        [Route("api/Book/")]
        public IHttpActionResult InsertBooks(Book data)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var book = new Book
            {
                ISBN = data.ISBN,
                Title = data.Title,
                Author = data.Author,
                Publisher = data.Publisher,
                PublishYear = data.PublishYear,
                PageNum = data.PageNum,
            };

            db.Books.Add(book);
            
            db.SaveChanges();
            
            return Ok();
        }

        public class GetRequest
        {
            public string ISBN;
        }

        [HttpGet]
        [Route("api/Book/1")]
        public IHttpActionResult GetBook(GetRequest data)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var book = db.Books.Find(data.ISBN);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }
    }
}