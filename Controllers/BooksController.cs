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
using EBook.Service;


namespace EBook.Controllers
{
    public class BookController : ApiController
    {
        private readonly OracleDbContext _db = new OracleDbContext();


        //insert update
        [HttpPost]
        [Route("api/Book/")]
        public IHttpActionResult InsertBooks(Book data)
        { 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_db.Books.Find(data.ISBN) == null)
            {
                Book book = new Book
                {
                    ISBN = data.ISBN,
                    Title = data.Title,
                    Author = data.Author,
                    Publisher = data.Publisher,
                    PublishYear = data.PublishYear,
                    PageNum = data.PageNum,
                    EBookKey = data.EBookKey,
                    ImagePath = data.ImagePath,
                    IsValid = 1,
                };


                _db.Books.Add(book);
                _db.SaveChanges();

                return Ok("书籍插入成功！");
            }

            var updatedbook = _db.Books.FirstOrDefault(b => b.ISBN == data.ISBN);
            if (updatedbook != null)
            {
                updatedbook.ISBN = data.ISBN;
                updatedbook.Author = data.Author;
                updatedbook.Publisher = data.Publisher;
                updatedbook.PublishYear = data.PublishYear;
                updatedbook.PageNum = data.PageNum;
                _db.SaveChanges();
                return Ok("书籍更新成功！");
            }

            return BadRequest("请重新操作！");
        }

        
        
        public class GetRequest
        {
            public readonly string ISBN;

            public GetRequest(string isbn)
            {
                ISBN = isbn;
            }
        }

        //get ok
        [HttpPost]
        [Route("api/GetBook")]
        public IHttpActionResult GetBook(GetRequest data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = _db.Books.Find(data.ISBN);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }
    }
}