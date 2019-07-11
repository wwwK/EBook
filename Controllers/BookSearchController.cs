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
             public readonly string Author;
             public readonly string Publisher;
             public readonly string ISBN;
             public readonly string ShopName;

             public SearchDate(string title, string author, string publisher, string isbn, string shopName)
             {
                 Title = title;
                 Author = author;
                 Publisher = publisher;
                 ISBN = isbn;
                 ShopName = shopName;
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

             int changed = 0;
             BookInfo[] books = BookSearch.BookSearchWithTitle("");
             if (data.Title != null)
             {
                 changed = 1;
                 books = books.Intersect(BookSearch.BookSearchWithTitle(data.Title)).ToArray();
             }
             if (data.Author != null)
             {
                 changed = 1;
                 books = books.Intersect(BookSearch.BookSearchWithAuther(data.Author)).ToArray();
             }
             if (data.Publisher != null)
             {
                 changed = 1;
                 books = books.Intersect(BookSearch.BookSearchWithPublisher(data.Publisher)).ToArray();
             }
             if (data.ISBN != null)
             {
                 changed = 1;
                 books = books.Intersect(BookSearch.BookSearchWithIsnb(data.ISBN)).ToArray();
             }
             if (data.Title != null)
             {
                 changed = 1;
                 books = books.Intersect(BookSearch.BookSearchWithShopName(data.ShopName)).ToArray();
             }
             
             if (books.Length == 0 || changed == 0)
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