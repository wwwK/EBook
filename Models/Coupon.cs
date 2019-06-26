using System;
using System.ComponentModel.DataAnnotations;

namespace EBook.Models
{
    public class Coupon
    {
       
        [Key]
        public int CouponId { set; get; }


        public int ReleaseBySellerId { set; get; }

        public int DiscountAmount { set; get; }

        public DateTime ValidThrough { set; get; }
        
        
        public int PriceLimit { set; get; }
        
        
        
        public string CouponStatus { set; get; }

      


    }
}