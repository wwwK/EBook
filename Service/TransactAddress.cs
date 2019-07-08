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
    public class TransactAddressInfo
    {
        public string ReceiverName;
        public string ReceivePhone;
        public string SourceProvince;
        public string SourceCity;
        public string SourceBlock;
        public string SourceDetailAddress;
        public int SourceZipCode;
        public string SellerPhone;
        public string DestinationProvince;
        public string DestinationCity;
        public string DestinationBlock;
        public int LogisticTrackNum;
    }
    
    public static class TransactAddress
    {
        public static OracleDbContext db = new OracleDbContext();

        public static TransactAddressInfo[] GetTransactAddress(int transactId)
        {
            CustomerAddress[] customerAddressesArray = db.CustomerAddresses.ToArray();
            SellerAddress[] sellerAddressesArray = db.SellerAddresses.ToArray();
            Transact[] transactsArray = db.Transacts.ToArray();
            IEnumerable<TransactAddressInfo> selectedAddressInfos =
                from customerAddress in customerAddressesArray
                join transact in transactsArray on customerAddress.AddressIndex equals transact.DestinationAddressIndex
                    into transactAddArray
                from transactAdd in transactAddArray
                join sellerAddress in sellerAddressesArray on transactAdd.SourceAdressIndex equals sellerAddress
                    .AddressIndex into sellerAddressArray
                from sellerAddress in sellerAddressesArray
                where transactAdd.TransactId == transactId // && transactAdd.Status > 0        //todo
                select new TransactAddressInfo
                {
                    ReceiverName = customerAddress.ReceiverName,
                    ReceivePhone = customerAddress.ReceivePhone,
                    SourceProvince = customerAddress.Province,
                    SourceCity = customerAddress.City,
                    SourceBlock = customerAddress.Block,
                    SourceDetailAddress = customerAddress.DetailAddress,
                    SourceZipCode = customerAddress.ZipCode,
                    SellerPhone = sellerAddress.Phone,
                    DestinationProvince = sellerAddress.Province,
                    DestinationCity = sellerAddress.City,
                    DestinationBlock = sellerAddress.Block,
                    LogisticTrackNum = transactAdd.LogisticTrackNum,

                };
            

            return selectedAddressInfos.ToArray();
        }
    }
}