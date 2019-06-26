using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBook.Models
{
    public class Transact
    {
        
        [Key]
        public int CouponId { set; get; }


        public int ReleaseBySellerId { set; get; }

        public int DiscountAmount { set; get; }

        public DateTime ValidThrough { set; get; }
        
        
        public int PriceLimit { set; get; }
        
        
        
        public string CouponStatus { set; get; }

/*

        [ForeignKey("CustomerId")] 
        public Customer Customer{ set; get; }

        public int CustomerId { set; get; }

        [Key]
        public int AddressId { set; get; }


        public int MerchandiseId { set; get; }


        public int ActualPrice { set; get; }


        public string Status { set; get; }


        public int Amount { set; get; }

        public DateTime CreateTime { set; get; }


        public int LogisticTrackNum { set; get; }


        public string Comment { set; get; }
        */
        
    }
}