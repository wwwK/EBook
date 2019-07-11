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
using NETCore.Encrypt;
using EBook.Service;


namespace EBook.Controllers
{
    public class DataProcessingController : ApiController
    {
        [HttpPost]
        [Route("api/GetBooksOfAllTransacts")]
        public IHttpActionResult GetBooksOfAllTransacts()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
 
             
            //var a = new BookSearch();
            Book[] books = DataProcessing.GetBooksOfAllTransacts();
            if (books.Length == 0)
            {
                return BadRequest("No Books Found");
            }

            return Ok(books);

        }
        
        
        
    }
}