using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using BCrypt.Net;

namespace EBook.Service
{
    public static class AddressService
    {
        public static OracleDbContext db = new OracleDbContext();

        public static CustomerAddress[] CustomerGetAllAddresses(int customerId)
        {
            CustomerAddress[] customerAddressesArray = db.CustomerAddresses.ToArray();
            IEnumerable<CustomerAddress> selectedCustomerAddressesArray =
                from customerAddress in customerAddressesArray
                where customerAddress.CustomerId == customerId
                select customerAddress;

            return selectedCustomerAddressesArray.ToArray();
        }
        
        public static SellerAddress[] SellerGetAllAddresses(int sellerId)
        {
            SellerAddress[] sellerAddressesArray = db.SellerAddresses.ToArray();
            IEnumerable<SellerAddress> selectedSellerAddressesArray =
                from sellerAddress in sellerAddressesArray
                where sellerAddress.SellerId == sellerId
                select sellerAddress;

            return selectedSellerAddressesArray.ToArray();
        }
    }
}