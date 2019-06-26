using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace EBook.Models
{
    public class CustomerAddress
    {
        
        
        
        [Key]
        public int AddressIndex { get; set; }

        
        public string ReceiverName { get; set; }
        

        public string ReceivePhone { get; set; }
        

        public string Province { get; set; }

        
        public string City { get; set; }


        public string Block { get; set; }

        
        
        public string DetailAddress { get; set; }


        
        public int ZipCode { get; set; }


        public int CustomerId{ get; set; }
        [ForeignKey("CustomerId")]
        public Customer User { set; get; }


        public bool IsDefault { get; set; }

        public CustomerAddress()
        {
            // 默认非 Default
            IsDefault = false;
        }



    }
}