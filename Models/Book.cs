using System;
using System.Collections.Generic;
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

        public DateTime PublishYear { get; set; }

        public int PageNum { get; set; }


    }
}