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
    public static class DataProcessing
    {
        public static OracleDbContext db = new OracleDbContext();

        public static Book[] GetBooksOfAllTransacts()
        {
            Transact[] transactsArray = db.Transacts.ToArray();
            Merchandise[] merchandisesArray = db.Merchandises.ToArray();
            Book[] booksArray = db.Books.ToArray();
            IEnumerable<Book> selectedBooks =
                from transact in transactsArray
                join merchandise in merchandisesArray on transact.MerchandiseId equals merchandise.MerchandiseId into
                    tranMerchanArray
                from tranMerchan in tranMerchanArray
                join book in booksArray on tranMerchan.ISBN equals book.ISBN
                select book;

            return selectedBooks.ToArray();
        }


        public class ShopStatistic
        {
            public string ShopName;
            public int Amount;
        }

        public static Dictionary<string, int> StatisticForAllSellers()
        {
            Transact[] transactsArray = db.Transacts.ToArray();
            Merchandise[] merchandisesArray = db.Merchandises.ToArray();
            Seller[] sellersArray = db.Sellers.ToArray();
            IEnumerable<ShopStatistic> statisticsArray =
                from transact in transactsArray
                join merchandise in merchandisesArray on transact.MerchandiseId equals merchandise.MerchandiseId
                join seller in sellersArray on merchandise.SellerId equals seller.SellerId
                select new ShopStatistic
                {
                    ShopName = seller.ShopName,
                    Amount = transact.Amount
                };
            ShopStatistic[] shops = statisticsArray.ToArray();
            Dictionary<string, int> statistic = new Dictionary<string, int>();
            foreach (ShopStatistic shopStatistic in statisticsArray)
            {
                if (statistic.ContainsKey(shopStatistic.ShopName))
                {
                    statistic[shopStatistic.ShopName] += shopStatistic.Amount;
                }
                else
                {
                    statistic.Add(shopStatistic.ShopName,0);
                    statistic[shopStatistic.ShopName] = shopStatistic.Amount;
                }
            }

            return statistic;
        }
    }
}