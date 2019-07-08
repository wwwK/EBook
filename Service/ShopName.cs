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
using System.Web.Http.Results;
using BCrypt.Net;

namespace EBook.Service
{
    public static class ShopNameService
    {
        public static OracleDbContext db = new OracleDbContext();
        
        public static int GetSellerIdByShopName(string name)
        {
            Seller[] sellersArray = db.Sellers.ToArray();
            IEnumerable<int> sellerId =
                from seller in sellersArray
                where seller.ShopName == name
                select seller.SellerId;

            if (sellerId == null)
            {
                return -1;
            }
            int[] fountSellerId = sellerId.ToArray();
            
    
                
            return fountSellerId[0];
        }

        public static bool CheckShopNameIfExist(string name)
        {
            Seller[] sellersArray = db.Sellers.ToArray();

            IEnumerable<int> sellerId =
                from seller in sellersArray
                where seller.ShopName == name
                select seller.SellerId;

            if (sellerId == null)
            {
                return true;
            }

            return false;
        }
    }

}