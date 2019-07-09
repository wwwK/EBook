using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EBook.Models;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using BCrypt.Net;


namespace EBook.Service
{
    public static class TransactService
    {
        //public class Transact[] 
        public static OracleDbContext db = new OracleDbContext();

        public static Transact[] SellerGetAllTransacts(int sellerId)
        {
            Merchandise[] merchandisesArray = db.Merchandises.ToArray();
            Transact[] transactsArray = db.Transacts.ToArray();
            IEnumerable<Transact> selectedTransacts =
                from transact in transactsArray
                join merchandise in merchandisesArray on transact.MerchandiseId equals merchandise.MerchandiseId into
                    tranMerchanArray
                from tranMerchan in tranMerchanArray
                where tranMerchan.SellerId == sellerId //&& tranMerchan.IsValid == 1
                select transact;
            return selectedTransacts.ToArray();
        }
        
        
        public static Transact[] CustomerGetAllTransacts(int customerId)
        {
            Transact[] transactsArray = db.Transacts.ToArray();
            IEnumerable<Transact> selectedTransacts =
                from transact in transactsArray
                where transact.CustomerId == customerId // && transact.Status 
                select transact;
            return selectedTransacts.ToArray();
        }

        public class CommentInfo
        {
            public string NickName;
            public int MerchandiseId;
            public string Comment;
            public DateTime CommentTime;
        }
        public static CommentInfo[] GetCommentsOfMerchandise(int merchandiseId)
        {
            Customer[] customersArray = db.Customers.ToArray();
            Merchandise[] merchandisesArray = db.Merchandises.ToArray();
            Transact[] transactsArray = db.Transacts.ToArray();
            IEnumerable<CommentInfo> selectedTransacts =
                from merchandise in merchandisesArray
                join transact in transactsArray on merchandise.MerchandiseId equals transact.MerchandiseId into
                    merchanTransactsArray
                from merchanTransact in merchanTransactsArray join customer in customersArray on merchanTransact.CustomerId equals customer.CustomerId into merchanCusArray
                from customer in customersArray
                where merchanTransact.MerchandiseId == merchandiseId && merchanTransact.Status > 0 
                select new CommentInfo
                {
                    NickName = customer.NickName,
                    MerchandiseId = merchandise.MerchandiseId,
                    Comment = merchanTransact.Comment,
                    CommentTime = merchanTransact.CommentTime,
                };
            return selectedTransacts.ToArray();
        }
    }
}