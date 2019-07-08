using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace EBook.Models
{
    public class SellerAddress
    {
        
        
        
        [Key]
        public int AddressIndex { get; set; }
        
        [MaxLength(20)]
        public string Phone { get; set; }
        

        [MaxLength(20)]
        public string Province { get; set; }

        [MaxLength(20)]
        public string City { get; set; }

        [MaxLength(20)]
        public string Block { get; set; }
        
        


        public int SellerId{ get; set; }
        [ForeignKey("SellerId")]
        public Seller User { set; get; }


   
        
        public int IsValid { get; set; }

        public SellerAddress()
        {
            IsValid = 1;
        }

    }
}