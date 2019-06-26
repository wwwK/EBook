using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBook.Models
{
    public class Own
    {
        [Key]
        [Column(Order=1)]
        public int CustomerId{ get; set; }
        [ForeignKey("CustomerId")]
        public Customer User { set; get; }
        
        [Key]
        [Column(Order = 2)]
        public int CouponId { get; set; }
        [ForeignKey("CouponId")]
        public Coupon Coupon { get; set; }

        [MaxLength(20)]
        public string Status { get; set; }

    }
}