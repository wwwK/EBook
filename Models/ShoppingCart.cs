using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;


namespace EBook.Models
{
    public class ShoppingCart
    {
       
        
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")] 
        public virtual Customer Customer { set; get; }


        [ForeignKey("MerchandiseId")]
        public int MerchandiseId { get; set; }

        public int Amount { get; set; }
        
        
    }
}