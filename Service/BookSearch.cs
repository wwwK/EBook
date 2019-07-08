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

namespace EBook.Service
{
    public class BookInfo
    {
        public string ISBN;
        public string Title;
        public string Author;
        public string Publisher;
        public DateTime PublishYear;
        public int PageNum;
        public string Description;
        public int Price;
        public string ShopName;
        public int SellerId;
    }

    public class BookSearch
    {
        public OracleDbContext db = new OracleDbContext();

        public BookInfo[] BookSearchWithTitle(string s)
        {
            Book[] booksArray = db.Books.ToArray();
            Merchandise[] merchandisesArray = db.Merchandises.ToArray();
            Seller[] sellersArray = db.Sellers.ToArray();
            IEnumerable<BookInfo> selectedBookInfos =
                from book in booksArray
                join merchandise in merchandisesArray on book.ISBN equals merchandise.ISBN into bookMerchandiseArray
                from bookMerchandise in bookMerchandiseArray
                join seller in sellersArray on bookMerchandise.SellerId equals seller.SellerId into bookInfoArray
                from bookInfo in bookInfoArray
                where book.Title.IndexOf(s) >= 0
                select new BookInfo
                {
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Author = book.Author,
                    //TODO
                    Publisher = book.Publisher,
                    PublishYear = book.PublishYear,
                    PageNum = book.PageNum,
                    Description = bookMerchandise.Description,
                    Price = bookMerchandise.Price,
                    SellerId = bookMerchandise.SellerId,
                    ShopName = bookInfo.ShopName,
                    
                };

            return selectedBookInfos.ToArray();
        }
        
        public BookInfo[] BookSearchWithAuther(string s)
        {
            Book[] booksArray = db.Books.ToArray();
            Merchandise[] merchandisesArray = db.Merchandises.ToArray();
            Seller[] sellersArray = db.Sellers.ToArray();
            IEnumerable<BookInfo> selectedBookInfos =
                from book in booksArray
                join merchandise in merchandisesArray on book.ISBN equals merchandise.ISBN into bookMerchandiseArray
                from bookMerchandise in bookMerchandiseArray
                join seller in sellersArray on bookMerchandise.SellerId equals seller.SellerId into bookInfoArray
                from bookInfo in bookInfoArray
                where book.Author.IndexOf(s) >= 0
                select new BookInfo
                {
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Author = book.Author,
                    Publisher = book.Publisher,
                    PublishYear = book.PublishYear,
                    PageNum = book.PageNum,
                    Description = bookMerchandise.Description,
                    Price = bookMerchandise.Price,
                    SellerId = bookMerchandise.SellerId,
                    ShopName = bookInfo.ShopName,
                    
                };

            return selectedBookInfos.ToArray();
        }
        
        public BookInfo[] BookSearchWithPublisher(string s)
        {
            Book[] booksArray = db.Books.ToArray();
            Merchandise[] merchandisesArray = db.Merchandises.ToArray();
            Seller[] sellersArray = db.Sellers.ToArray();
            IEnumerable<BookInfo> selectedBookInfos =
                from book in booksArray
                join merchandise in merchandisesArray on book.ISBN equals merchandise.ISBN into bookMerchandiseArray
                from bookMerchandise in bookMerchandiseArray
                join seller in sellersArray on bookMerchandise.SellerId equals seller.SellerId into bookInfoArray
                from bookInfo in bookInfoArray
                where book.Publisher.IndexOf(s) >= 0
                select new BookInfo
                {
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Author = book.Author,
                    Publisher = book.Publisher,
                    PublishYear = book.PublishYear,
                    PageNum = book.PageNum,
                    Description = bookMerchandise.Description,
                    Price = bookMerchandise.Price,
                    SellerId = bookMerchandise.SellerId,
                    ShopName = bookInfo.ShopName,
                    
                };

            return selectedBookInfos.ToArray();
        }
        
        public BookInfo[] BookSearchWithIsnb(string s)
        {
            Book[] booksArray = db.Books.ToArray();
            Merchandise[] merchandisesArray = db.Merchandises.ToArray();
            Seller[] sellersArray = db.Sellers.ToArray();
            IEnumerable<BookInfo> selectedBookInfos =
                from book in booksArray
                join merchandise in merchandisesArray on book.ISBN equals merchandise.ISBN into bookMerchandiseArray
                from bookMerchandise in bookMerchandiseArray
                join seller in sellersArray on bookMerchandise.SellerId equals seller.SellerId into bookInfoArray
                from bookInfo in bookInfoArray
                where book.ISBN.IndexOf(s) >= 0
                select new BookInfo
                {
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Author = book.Author,
                    Publisher = book.Publisher,
                    PublishYear = book.PublishYear,
                    PageNum = book.PageNum,
                    Description = bookMerchandise.Description,
                    Price = bookMerchandise.Price,
                    SellerId = bookMerchandise.SellerId,
                    ShopName = bookInfo.ShopName,
                    
                };

            return selectedBookInfos.ToArray();
        }
        
        public BookInfo[] BookSearchWithShopName(string s)
        {
            Book[] booksArray = db.Books.ToArray();
            Merchandise[] merchandisesArray = db.Merchandises.ToArray();
            Seller[] sellersArray = db.Sellers.ToArray();
            IEnumerable<BookInfo> selectedBookInfos = 
                from book in booksArray
                join merchandise in merchandisesArray on book.ISBN equals merchandise.ISBN into bookMerchandiseArray
                from bookMerchandise in bookMerchandiseArray
                join seller in sellersArray on bookMerchandise.SellerId equals seller.SellerId into bookInfoArray
                from bookInfo in bookInfoArray
                where bookMerchandise.Seller.ShopName.IndexOf(s) >= 0
                select new BookInfo
                {
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Author = book.Author,
                    Publisher = book.Publisher,
                    PublishYear = book.PublishYear,
                    PageNum = book.PageNum,
                    Description = bookMerchandise.Description,
                    Price = bookMerchandise.Price,
                    SellerId = bookMerchandise.SellerId,
                    ShopName = bookInfo.ShopName,
                    
                };

            return selectedBookInfos.ToArray();
        }
    }
}