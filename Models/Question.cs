using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBook.Models
{
    public class Question
    {
       
        
        [Key]
        public int QuestionId { get; set; }
        
        
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }


        public int AboutMerchandiseId { get; set; }
        [ForeignKey("AboutMerchandiseId")]
        public Merchandise Merchandise { get; set; }
        

        public DateTime SubmitTime { get; set; }

        [MaxLength(200)]
        public string Content { get; set; }

        // public ICollection<Answer> Answers { set; get; }
    }
}