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
        public int MerchandiseId;
        public string ISBN;
        public string Title;
        public string Author;
        public string Publisher;
        public int PublishYear;
        public int PageNum;
        public string Description;
        public int Price;
        public string ShopName;
    }

    public static class BookSearch
    {
        public static OracleDbContext db = new OracleDbContext();

        public static BookInfo[] BookSearchWithTitle(string s)
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
                where book.Title.IndexOf(s) >= 0 && bookMerchandise.IsValid == 1
                select new BookInfo
                {
                    MerchandiseId = bookMerchandise.MerchandiseId,
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Author = book.Author,
                    //TODO
                    Publisher = book.Publisher,
                    PublishYear = book.PublishYear,
                    PageNum = book.PageNum,
                    Description = bookMerchandise.Description,
                    Price = bookMerchandise.Price,
                    ShopName = bookInfo.ShopName,
                    
                };

            return selectedBookInfos.ToArray();
        }
        
        public static BookInfo[] BookSearchWithAuther(string s)
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
                    MerchandiseId = bookMerchandise.MerchandiseId,
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Author = book.Author,
                    Publisher = book.Publisher,
                    PublishYear = book.PublishYear,
                    PageNum = book.PageNum,
                    Description = bookMerchandise.Description,
                    Price = bookMerchandise.Price,
                    ShopName = bookInfo.ShopName,
                    
                };

            return selectedBookInfos.ToArray();
        }
        
        public static BookInfo[] BookSearchWithPublisher(string s)
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
                    MerchandiseId = bookMerchandise.MerchandiseId,
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Author = book.Author,
                    Publisher = book.Publisher,
                    PublishYear = book.PublishYear,
                    PageNum = book.PageNum,
                    Description = bookMerchandise.Description,
                    Price = bookMerchandise.Price,
                    ShopName = bookInfo.ShopName,
                    
                };

            return selectedBookInfos.ToArray();
        }
        
        public static BookInfo[] BookSearchWithIsnb(string s)
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
                    MerchandiseId = bookMerchandise.MerchandiseId,
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Author = book.Author,
                    Publisher = book.Publisher,
                    PublishYear = book.PublishYear,
                    PageNum = book.PageNum,
                    Description = bookMerchandise.Description,
                    Price = bookMerchandise.Price,
                    ShopName = bookInfo.ShopName,
                    
                };

            return selectedBookInfos.ToArray();
        }
        
        public static BookInfo[] BookSearchWithShopName(string s)
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
                    MerchandiseId = bookMerchandise.MerchandiseId,
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Author = book.Author,
                    Publisher = book.Publisher,
                    PublishYear = book.PublishYear,
                    PageNum = book.PageNum,
                    Description = bookMerchandise.Description,
                    Price = bookMerchandise.Price,
                    ShopName = bookInfo.ShopName,
                    
                };

            return selectedBookInfos.ToArray();
        }
        
        public static BookInfo[] BookSearchWithMerchandiseId(int merchandiseId)
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
                where bookMerchandise.MerchandiseId == merchandiseId
                select new BookInfo
                {
                    MerchandiseId = bookMerchandise.MerchandiseId,
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Author = book.Author,
                    Publisher = book.Publisher,
                    PublishYear = book.PublishYear,
                    PageNum = book.PageNum,
                    Description = bookMerchandise.Description,
                    Price = bookMerchandise.Price,
                    ShopName = bookInfo.ShopName,
                    
                };

            return selectedBookInfos.ToArray();
        }
    }
}