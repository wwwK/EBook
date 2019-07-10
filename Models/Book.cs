using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBook.Models
{
    public class Book
    {
        [Key]
        [MaxLength(20)]
        public string ISBN { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(20)]
        public string Author { get; set; }

        [MaxLength(20)]
        public string Publisher { get; set; }

        
        [Url]
        [MaxLength(100)]
        public string ImagePath { set; get; }
        
        
        public int PublishYear { get; set; }

        public int PageNum { get; set; }


        [MaxLength(30)]
        public string EBookKey { set;get; }
        
        //[DefaultValue(1)]
        public int IsValid { get; set; }

        public Book()
        {
            IsValid = 1;
        }
    }
}