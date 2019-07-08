using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace EBook.Models
{
    public class SellerAddress
    {
        
        
        
        [Key]
        public int AddressIndex { get; set; }
        
        
        public string Phone { get; set; }
        

        public string Province { get; set; }

        
        public string City { get; set; }


        public string Block { get; set; }
        
        


        public int CustomerId{ get; set; }
        [ForeignKey("CustomerId")]
        public Customer User { set; get; }


   



    }
}