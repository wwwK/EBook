using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBook.Models
{
    public class AnswerFor
    {


        [Key] 
        public int AnswerId;
        /*
        [Key]
        [ForeignKey("QAId")]
          public QA QA { get; set; }
          public int AnswerId { get; set; }
  
          [ForeignKey("QAId")]
          
          public int QuestionId { get; set; }
  */

    }
}