using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EBook.Models
{
    public class OrderContainsTransacts
    {
        [Key]
        public int OrderId { get; set; }
        
        [Key]
        public int TransactId { get; set; }
        
    }
}