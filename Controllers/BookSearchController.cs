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
 
 //TODO
 namespace EBook.Controllers
 {
     
     public class BookSearchController : ApiController
     {



         public class SearchDate
         {
             public readonly string Title;

             public SearchDate(string title)
             {
                 Title = title;
             }
         }

         
         
         
         
 
         [HttpPost]
         [Route("api/BookSearch/")]
         public IHttpActionResult Search(SearchDate data)
         {
             if (!ModelState.IsValid)
             {
                 return BadRequest(ModelState);
             }

             
             BookInfo[] books = BookSearch.BookSearchWithTitle(data.Title);
             
             if (books.Length == 0 )
             {
                 return BadRequest("没有找到书籍！");
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