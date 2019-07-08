using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBook.Models
{
    public class Seller
    {
        [Key]
        public int SellerId { get; set; }

        [MaxLength(1000)]
        public string Password { get; set; }

        
        [Index(IsUnique = true)]
        [MaxLength(50)]
        public string ShopName { get; set; }

        
        public int CreditLevel { get; set; }

        [MaxLength(200)]
        public string ShopDescription { get; set; }
        
        
        
        [MaxLength(60)]
        public string AvatarPath { set; get; }

        
        
        public int DefaultSellerAddressIndex { get; set; }
        
        
        
        [Required]
        [MaxLength(20)]
        [EmailAddress]
        public string SellerEmail { get; set; }

        [Phone]
        [Index]
        [MaxLength(11)]
        public string SellerPhone { get; set; }
        
        
        
        
        public int IsValid { get; set; }


        public Seller()
        {
            IsValid = 1;
        }
    }
}