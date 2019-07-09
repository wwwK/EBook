using System.Threading.Tasks;
 using System.Web;
 using System.Web.Http;
 using EBook.Models;
 using System;
 using EBook.Service;
 using System.Linq;
 using System.Net;
 using System.Net.Http;
 using System.Net.Http.Headers;
 using BCrypt.Net;
 
 
 namespace EBook.Controllers
 {
     
     public class BookSearchController : ApiController
     {



         public class SearchDate
         {
             public string Title;
         }

         //记得改！
         
 
         [HttpPost]
         [Route("api/BookSearch/")]
         public IHttpActionResult Search(SearchDate data)
         {
             if (!ModelState.IsValid)
             {
                 return BadRequest(ModelState);
             }
 
             
             //var a = new BookSearch();
             BookInfo[] books = BookSearch.BookSearchWithTitle(data.Title);
             if (books.Length == 0)
             {
                 return BadRequest("No Books Found");
             }
             return Ok(books);
         }
         
         /*[HttpGet]
         [Route("api/BookSearch/{SearchString}")]
         public async Task<IHttpActionResult> mohuchhhh(SearchDate data)
         {
             BookSearch a = new BookSearch();
             BookInfo[] css = a.BookSearchWithTitle(data.searchinfo);
             return Ok(css);
         }*/
 
         
     }
 
 }