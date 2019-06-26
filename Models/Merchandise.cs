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

        
        public int SellBySellerId { set; get; }



        public string BookIsbn { set; get; }



        public string Description { set; get; }



        public int Price { set; get; }

        

       

    }
}



    
