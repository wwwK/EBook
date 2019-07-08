using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBook.Models
{
    public class Answer
    {
        

        [Key]
        public int AnswerId { get; set; }
        
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        
        public int QuestionAnsweredId { get; set; }
        [ForeignKey("QuestionAnsweredId")]
        public Question QuestionAnswered { get; set; }

        public DateTime SubmitTime { get; set; }

        [MaxLength(200)]
        public string Content { get; set; }
        
        public bool IsValid { get; set; }

        public Answer()
        {
            IsValid = true;
        }

        
    }
}