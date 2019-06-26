using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations;

namespace EBook.Migrations
{
    public class Init :DbMigration
    {
        public override void Up()
        {
          /* 
            CreateTable(
                    "EBook.Customers",
                    c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName  = c.String(maxLength: 20),
                        NickName = c.String(maxLength: 20),
                        PasswordHash = c.String(maxLength: 40),
                        DefaultAddressIndex = c.Int(),
                        IdCardNum = c.String(maxLength: 40),
                        PhoneNum   = c.Int(identity: true),
                        DateOfBirth  = c.DateTime(),
                        Point =  c.Int(),  
                    })
                .PrimaryKey(a => a.CustomerId);
            
        /*    
            
            CreateTable(
                    "EBook.CustomerAddresses",
                    c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName  = c.String(maxLength: 20),
                        NickName = c.String(maxLength: 20),
                        Password = c.String(maxLength: 40),
                        DefaultAddressIndex = c.Int(),
                        IdCardNum = c.String(maxLength: 40),
                        PhoneNum   = c.Int(identity: true),
                        DateOfBirth  = c.DateTime(),
                        Point =  c.Int(),  
                    })
                .PrimaryKey(a => a.CustomerId);

*/

        }


        public override void Down()
        {
           // DropTable("EBook.Customers");
            
            
            
            
        }
    }
}