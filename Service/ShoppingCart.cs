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
        public int SellerId;
        public string ISBN;
        public string Description;
        public int Price;
        public bool IsValid;
        public string ImagePath;
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
                where shoppingcart.CustomerId == customerId & shoppingcart.Amount > 0
                select new MerchandiseInfo
                {
                    MerchandiseId = cartMerchandise.MerchandiseId,
                    SellerId = cartMerchandise.SellerId,
                    ISBN = cartMerchandise.ISBN,
                    Description = cartMerchandise.Description,
                    Price = cartMerchandise.Price,
                    IsValid = cartMerchandise.IsValid,
                    ImagePath = cartMerchandise.ImagePath,
                    ShopName = merchandiseSeller.ShopName,
                    Amount = shoppingcart.Amount,
                };
            return selectedMerchandiseInfo.ToArray();
        }
    }
}