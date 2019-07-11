using System;
using System.ComponentModel;
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

        public double Price { set; get; }
        
        
        
        public int IsValid { get; set; }
        
        
        [MaxLength(50)]
        public string ImagePath1 { set; get; }
        
        [MaxLength(50)]
        public string ImagePath2 { set; get; }
        
        [MaxLength(50)]
        public string ImagePath3 { set; get; }
        
        
        [MaxLength(50)]
        public string ImagePath4 { set; get; }
        
        [MaxLength(50)]
        public string ImagePath5 { set; get; }
        
        
        [MaxLength(50)]
        public string VideoPath { set; get; }




        public Merchandise()
        {
            IsValid = 1;
        }

    }
}



    
