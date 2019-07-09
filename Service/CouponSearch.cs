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
    public class CouponInfo
    {
        public string ShopName;
        public int CouponId;
        public int SellerId;
        public int DiscountAmount;
        public DateTime ValidThrough;
        public int PriceLimit;
        public string CouponState;
    }

    public static class CouponSearch
    {
        public static OracleDbContext db = new OracleDbContext();

        public static CouponInfo[] CouponSearchWithShopName(string s)
        {
            Coupon[] couponsArray = db.Coupons.ToArray(); 
            Seller[] sellersArray = db.Sellers.ToArray();
            IEnumerable<CouponInfo> selectedCouponInfos =
                from coupon in couponsArray
                join seller in sellersArray on coupon.ReleaseBySellerId equals seller.SellerId into shopCouponsArray
                from shopCoupon in shopCouponsArray
                where shopCoupon.ShopName.IndexOf(s) >= 0 && shopCoupon.IsValid == 1
                select new CouponInfo
                {
                    ShopName = shopCoupon.ShopName,
                    CouponId = coupon.CouponId,
                    //SellerId = coupon.ReleaseBySellerId,
                    DiscountAmount = coupon.DiscountAmount,
                    ValidThrough = coupon.ValidThrough,
                    PriceLimit = coupon.PriceLimit
                };

            return selectedCouponInfos.ToArray();
        }
        public static CouponInfo[] GetAllCouponsWithSellerId(int sellerId)
        {
            Coupon[] couponsArray = db.Coupons.ToArray(); 
            Seller[] sellersArray = db.Sellers.ToArray();
            IEnumerable<CouponInfo> selectedCouponInfos =
                from coupon in couponsArray
                join seller in sellersArray on coupon.ReleaseBySellerId equals seller.SellerId into sellerCouponsArray
                from sellerCoupon in sellerCouponsArray
                where sellerCoupon.SellerId == sellerId && sellerCoupon.IsValid == 1
                //select coupon;
                select new CouponInfo
                {
                    ShopName = sellerCoupon.ShopName,
                    CouponId = coupon.CouponId,
                    //SellerId = coupon.ReleaseBySellerId,
                    DiscountAmount = coupon.DiscountAmount,
                    ValidThrough = coupon.ValidThrough,
                    PriceLimit = coupon.PriceLimit
                };

            return selectedCouponInfos.ToArray();
        }
        
        
        
        
    }
}