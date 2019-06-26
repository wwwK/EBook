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

//        [Key]
//        public int MerchandiseId { set; get; }
//        public int SellerId { set; get; }
//        [ForeignKey("SellerId")]
//        public Seller Seller { get; set; }


//        public string Isbn { set; get; }
//        [ForeignKey("Isbn")]
//        public Book Book { set; get; }

        //public string Description { set; get; }

        //public int Price { set; get; }

    }
}



    
