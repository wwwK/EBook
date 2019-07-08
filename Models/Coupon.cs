using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBook.Models
{
    public class Coupon
    {
       
        [Key]
        public int CouponId { set; get; }
        
        public int ReleaseBySellerId { set; get; }
        [ForeignKey("ReleaseBySellerId")]
        public Seller ReleaseBy { set; get; }

        public int DiscountAmount { set; get; }

        public DateTime ValidThrough { set; get; }
        
        public int PriceLimit { set; get; }
        
        
                
        
  
        

        public int IsValid { get; set; }

        public Coupon()
        {
            IsValid = 1;
        }
        
    }
}