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

        public static Transact[] GetAllTransacts(int sellerId)
        {
            Merchandise[] merchandisesArray = db.Merchandises.ToArray();
            Transact[] transactsArray = db.Transacts.ToArray();
            IEnumerable<Transact> selectedTransacts=
                from transact in transactsArray
                join merchandise in merchandisesArray on transact.MerchandiseId equals merchandise.MerchandiseId into
                    tranMerchanArray
                from tranMerchan in tranMerchanArray
                where tranMerchan.SellerId == sellerId
                select transact;
            return selectedTransacts.ToArray();
        }

        
    }
}