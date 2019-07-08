using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBook.Models
{
    public class Transact
    {
        
        /*[Key]
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
        
        public string Comment { set; get; }*/

        
        [Key]
        public int TransactId { set; get; }
        
        
        public int CustomerId { set; get; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        
        public int MerchandiseId { set; get; }
        [ForeignKey("MerchandiseId")]
        public Merchandise Merchandise { get; set; }
        
        public int SourceAdressIndex { set; get; }
        [ForeignKey("SourceAddressIndex")]
        public SellerAddress SourceAddress { get; set; }
        
        public int DestinationAddressIndex { set; get; }
        [ForeignKey("DestinationAddressIndex")]
        public CustomerAddress Destination { get; set; }
        
        
        public int UsedCouponId { set; get; }
        //可能不使用优惠券，不设置外键参照约束
        
        public DateTime CreateTime { get; set; }
        
        public int ActualPrice { set; get; }
        
        
        // 1未付款  2未发货  3未收货   4收货   5评价 0 取消
        public int Status { set; get; }
        
        public int Amount { set; get; }

        public int LogisticTrackNum { set; get; }
        
        
        [MaxLength(200)]
        public string Comment { set; get; }
        
        [MaxLength(50)]
        public string ImagePath1 { set; get; }
        
        [MaxLength(50)]
        public string ImagePath2 { set; get; }
        
        public DateTime CommentTime { set; get; }
        
        
        
        
    }
}