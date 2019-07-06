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
     
     public class SearchController : ApiController
     {



         public class searchDate
         {
             public string searchinfo;
         }


         
 
         [HttpPost]
         [Route("api/Search/")]
         public async Task<IHttpActionResult> Mo(searchDate data)
         {
 
             
             BookSearch a = new BookSearch();
             BookInfo[] css = a.BookSearchWithTitle(data.searchinfo);
             return Ok(a.BookSearchWithTitle(data.searchinfo));
         }
         
         [HttpGet]
         [Route("api/Search/{SearchString}")]
         public async Task<IHttpActionResult> mohuchhhh(searchDate data)
         {
             BookSearch a = new BookSearch();
             BookInfo[] css = a.BookSearchWithTitle(data.searchinfo);
             return Ok(css);
         }
 
         
     }
 
 }