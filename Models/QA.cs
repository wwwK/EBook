using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBook.Models
{
    public class QA
    {
       
        
        [Key]
        public int QAId { get; set; }
        
        
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }


        public int AboutMerchandiseId { get; set; }
        [ForeignKey("MerchandiseId")]
        public virtual Merchandise Merchandise { get; set; }
        

        public DateTime SubmitTime { get; set; }

        [MaxLength(200)]
        public string Content { get; set; }

        [MaxLength(1)]
        public char Type { get; set; }
        
        
    }
}