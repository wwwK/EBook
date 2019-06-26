using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBook.Models
{
    public class Transact
    {
        
        [Key]
        [Column(Order=1)]
        public int CustomerId { set; get; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [Key]
        [Column(Order=2)]
        public int MerchandiseId { set; get; }
        [ForeignKey("MerchandiseId")]
        public Merchandise Merchandise { get; set; }
        
        [Key]
        [Column(Order=3)]
        public DateTime CreateTime { get; set; }
        
        public int ActualPrice { set; get; }
        
        public string Status { set; get; }
        
        public int Amount { set; get; }

        public int LogisticTrackNum { set; get; }
        
        public string Comment { set; get; }

/*

        [ForeignKey("CustomerId")] 
        public Customer Customer{ set; get; }

        public int CustomerId { set; get; }

        [Key]
        public int AddressId { set; get; }


        public int MerchandiseId { set; get; }


        public int ActualPrice { set; get; }


        public string Status { set; get; }


        public int Amount { set; get; }

        public DateTime CreateTime { set; get; }


        public int LogisticTrackNum { set; get; }


        public string Comment { set; get; }
        */
        
    }
}