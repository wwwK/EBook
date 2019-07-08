using System.Linq;
using System.Web.UI.WebControls;
using EBook.Models;

namespace EBook.Service
{
    public static class VipCheck
    {
        private static OracleDbContext db = new OracleDbContext();

        public class CustomerVipInfo
        {
            public Customer customer;
            public VipMember vipInfo;
        }
        
        public class SellerVipInfo
        {
            public Seller seller;
            public VipMember vipInfo;
        }
        
        public static CustomerVipInfo[] GetVipMemberFromSeller(int sellerId)
        {
            var result =
                from seller in db.Sellers
                join vipData in db.VipMembers on seller.SellerId equals vipData.SellerId
                join customer in db.Customers on vipData.CustomerId equals customer.CustomerId
                where seller.SellerId == sellerId
                select new CustomerVipInfo()
                {
                    customer = customer,
                    vipInfo = vipData
                };

            return result.ToArray();
        }
        
        public static SellerVipInfo[] GetVipMemberFromCustomer(int customerId)
        {
            var result =
                from seller in db.Sellers
                join vipData in db.VipMembers on seller.SellerId equals vipData.SellerId
                join customer in db.Customers on vipData.CustomerId equals customer.CustomerId
                where customer.CustomerId == customerId
                select new SellerVipInfo()
                {
                    seller = seller,
                    vipInfo = vipData
                };

            return result.ToArray();
        }
    }
}