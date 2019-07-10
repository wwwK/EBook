using System.Threading.Tasks;
using System.Web;
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
    public class ManageBookController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [HttpPost]
        [Route("api/ManageSelectBook")]
        public IHttpActionResult ManageSelectBook()
        {
            Book[] tableBooks = db.Books.ToArray();
            IEnumerable<Book> selectTableBooks =
                from book in tableBooks
                select book;
            return Ok(selectTableBooks.ToArray());

        }
        
        
        [HttpPost]
        [Route("api/ManageInsertSeller")]
        public IHttpActionResult ManageInsertBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Books.Add(book);
            db.SaveChanges();

            return Ok("Insert Success");
        }
        
    }
}