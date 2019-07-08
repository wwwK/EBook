using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBook.Models
{
    public class Collect
    {
        [Key]
        [Column(Order=1)]
        public int CustomerId{ get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { set; get; }
        
        [Key]
        [Column(Order=2)]
        public int MerchandiseId { get; set; }
        [ForeignKey("MerchandiseId")]
        public Merchandise Merchandise { get; set; }


        public DateTime CollectTime { get; set; }
        

    
        public int IsValid { get; set; }

        public Collect()
        {
            IsValid = 1;
        }

    }
}