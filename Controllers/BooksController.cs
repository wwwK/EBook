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
        public async Task<IHttpActionResult> InsertBooks(Book data)
        {
            Book book = new Book
            {
                ISBN = data.ISBN,
                Title = data.Title,
                Author = data.Author,
                Publisher = data.Publisher,
                PublishYear = data.PublishYear,
                PageNum = data.PageNum,
            };


            db.Books.Add(book);
            

            await db.SaveChangesAsync();
            

            return Ok();
        }

        public class GetRequest
        {
            public string ISBN;
        }

        [HttpGet]
        [Route("api/Book/1")]
        public async Task<IHttpActionResult> GetBook(GetRequest data)
        {
            var book = await db.Books.FindAsync(data.ISBN);
            if (book == null)
            {
                throw new HttpException(404, "User not found");
            }

            return Ok(book);
        }
    }
}