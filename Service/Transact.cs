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
                where tranMerchan.SellerId == sellerId && tranMerchan.IsValid == 1
                select transact;
            return selectedTransacts.ToArray();
        }

        public class CommentInfo
        {
            public int MerchandiseId;
            public string Comment;
            public DateTime CommentTime;
        }
        public static CommentInfo[] GetCommentsOfMerchandise(int merchandiseId)
        {
            Merchandise[] merchandisesArray = db.Merchandises.ToArray();
            Transact[] transactsArray = db.Transacts.ToArray();
            IEnumerable<CommentInfo> selectedTransacts =
                from transact in transactsArray
                join merchandise in merchandisesArray on transact.MerchandiseId equals merchandise.MerchandiseId into
                    tranMerchanArray
                from tranMerchan in tranMerchanArray
                where tranMerchan.MerchandiseId == merchandiseId && tranMerchan.IsValid == 1
                select new CommentInfo
                {
                    MerchandiseId = tranMerchan.MerchandiseId,
                    Comment = transact.Comment,
                    CommentTime = transact.CommentTime,
                };
            return selectedTransacts.ToArray();
        }
    }
}