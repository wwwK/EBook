using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBook.Models
{
    public class VipMember
    {
        [Key]
        [Column(Order=1)]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { set; get; }
        //
        [Key]
        [Column(Order=2)]
        public int SellerId { get; set; }
        [ForeignKey("SellerId")]
        public Seller Seller { set; get; }
        
        public double DiscountRatio { set; get; }
        
        public DateTime ValidThrough { set; get; }
      

    }
}