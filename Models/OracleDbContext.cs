using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EBook.Models
{
    public class OracleDbContext: DbContext
    {
        public OracleDbContext() : base("name=OracleDbContext")
        {
        }
        
        
        
        public DbSet<Customer> Customers { set; get; }
        
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        
        public DbSet<Seller>Sellers{ get; set; }
        
        public DbSet<Book> Books{ get; set; }
        
        public DbSet<Merchandise> Merchandises { get; set; }
        
         public DbSet<Transact> Transacts { get; set; }
        
         public DbSet<Coupon>Coupons { get; set; }

        public DbSet<Answer> Answers { get; set; }
        
        public DbSet<Question> Questions { get; set; }
           
        
        
        public DbSet<Collect>Collects{ get; set; }
        
        public DbSet<Own>Owns{ get; set; }
        
        public  DbSet<ShoppingCart>ShoppingCarts{ get; set; }

        public DbSet<VipMember> VipMembers { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            

            
            modelBuilder.HasDefaultSchema("EBOOK");
        }
    }
}