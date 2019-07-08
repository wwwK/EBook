using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBook.Models
{
    public class Seller
    {
        [Key]
        public int SellerId { get; set; }

        [MaxLength(20)]
        public string Password { get; set; }

        
        //todo unique
        [MaxLength(50)]
        public string ShopName { get; set; }

        
        public int CreditLevel { get; set; }

        [MaxLength(500)]
        public string ShopDescription { get; set; }

        [MaxLength(20)]
        public string City { get; set; }

        

        [Phone]
        [Index]
        [MaxLength(11)]
        public string SellerPhone { get; set; }
        
        
    }
}