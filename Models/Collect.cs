using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBook.Models
{
    public class Collect
    {
        public int CustomerId{ get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer User { set; get; }
        
        
        
        
        public int MerchandiseId { get; set; }
        [ForeignKey("MerchandiseId")]
        public virtual Merchandise Merchandise { get; set; }


        public DateTime CollectTime { get; set; }


    }
}