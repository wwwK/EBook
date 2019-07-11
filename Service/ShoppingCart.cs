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
    public class MerchandiseInfo
    {
        public int MerchandiseId;
        public string ISBN;
        public string Title;
        public string Description;
        public double Price;
        public string ImagePath1;
        public string ImagePath2;
        public string ImagePath3;
        public string ImagePath4;
        public string ImagePath5;
        public string ShopName;
        public int Amount;
    }
    public class SeeShoppingCart
    {
        public OracleDbContext db = new OracleDbContext();
        
        public MerchandiseInfo[] CheckShoppingCartWithCustomerId(int customerId)
        {
            ShoppingCart[] shoppingCartsArray = db.ShoppingCarts.ToArray();
            Seller[] sellersArray = db.Sellers.ToArray();
            Merchandise[] merchandisesArray = db.Merchandises.ToArray();
            IEnumerable<MerchandiseInfo> selectedMerchandiseInfo =
                from shoppingcart in shoppingCartsArray
                join merchandise in merchandisesArray on shoppingcart.MerchandiseId equals merchandise.MerchandiseId
                    into cartMerchandiseArray
                from cartMerchandise in cartMerchandiseArray
                join seller in sellersArray on cartMerchandise.SellerId equals seller.SellerId into
                    merchandiseSellerArray
                from merchandiseSeller in merchandiseSellerArray
                where shoppingcart.CustomerId == customerId & shoppingcart.Amount > 0 && cartMerchandise.IsValid == 1
                select new MerchandiseInfo
                {
                    MerchandiseId = cartMerchandise.MerchandiseId,
                    ISBN = cartMerchandise.ISBN,
                    Description = cartMerchandise.Description,
                    Price = cartMerchandise.Price,
                    ImagePath1 = cartMerchandise.ImagePath1,
                    ImagePath2 = cartMerchandise.ImagePath2,
                    ImagePath3 = cartMerchandise.ImagePath3,
                    ImagePath4 = cartMerchandise.ImagePath4,
                    ImagePath5 = cartMerchandise.ImagePath5,
                    ShopName = merchandiseSeller.ShopName,
                    Amount = shoppingcart.Amount,
                };
            return selectedMerchandiseInfo.ToArray();
        }
    }
}