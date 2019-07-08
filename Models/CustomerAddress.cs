using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace EBook.Models
{
    public class CustomerAddress
    {
        
        
        
        [Key]
        public int AddressIndex { get; set; }

        [MaxLength(30)]
        public string ReceiverName { get; set; }
        
        [Phone]
        public string ReceivePhone { get; set; }
        
        [MaxLength(20)]
        public string Province { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(50)]
        public string Block { get; set; }

        
        [MaxLength(50)]
        public string DetailAddress { get; set; }

        
        public int ZipCode { get; set; }


        
        [DefaultValue(1)]
        public int IsValid { get; set; }
        
        
        public int CustomerId{ get; set; }
        [ForeignKey("CustomerId")]
        public Customer User { set; get; } 
        

    }
}