using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace EBook.Models
{
    public class Merchandise
    {
      

        [Key]
        public int MerchandiseId { set; get; }
        public int SellerId { set; get; }
        [ForeignKey("SellerId")]
        public Seller Seller { get; set; }


        public string ISBN { set; get; }
        [ForeignKey("ISBN")]
        public Book Book { set; get; }

        public string Description { set; get; }

        public int Price { set; get; }
        
        
        public bool IsValid { set; get; }
        
        [MaxLength(50)]
        public string ImagePath { set; get; }

        public Merchandise()
        {
            IsValid = true;
        }

    }
}



    
