using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBook.Models
{
    public class Own
    {
       
        public int CustomerId{ get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer User { set; get; }

        

        
        public int CouponId { get; set; }
        [ForeignKey("CouponId")]
        public virtual Coupon Coupon { get; set; }

        [MaxLength(20)]
        public string Status { get; set; }

    }
}