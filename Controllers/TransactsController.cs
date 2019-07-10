﻿using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EBook.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using BCrypt.Net;
using System.Web.SessionState;
using EBook.Service;


namespace EBook.Controllers
{
    public class TransactCartController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        //insert update
        [HttpPost]
        [Route("api/Transact/")]
        public IHttpActionResult InsertTransact(Transact data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return BadRequest("Not Login");
            }

            int customerId = CustomerSession.GetCustomerIdFromSession(int.Parse(session.Value));
            if (customerId < 0)
            {
                return BadRequest("Not Login");
            }

            if (db.Transacts.Find(data.TransactId) == null)
            {
                Transact transact = new Transact
                {
                    CustomerId = customerId,
                    MerchandiseId = data.MerchandiseId,
                    CreateTime = DateTime.Now,
                    UsedCouponId = data.UsedCouponId,
                    ActualPrice = data.ActualPrice,
                    Status = 0,
                    Amount = data.Amount,
                    LogisticTrackNum = null,
                    Comment = null,
                    CommentTime = new DateTime(1970,1,1),
                };


                db.Transacts.Add(transact);
                db.SaveChanges();

                return Ok("Insert Success");
            }

            var updatetransact = db.Transacts.FirstOrDefault(t => t.TransactId == data.TransactId);
            if (updatetransact != null)
            {
                updatetransact.UsedCouponId = data.UsedCouponId;
                updatetransact.ActualPrice = data.ActualPrice;
                updatetransact.Status = data.Status;
                updatetransact.Amount = data.Amount;
                updatetransact.LogisticTrackNum = data.LogisticTrackNum;
                updatetransact.Comment = data.Comment;
                updatetransact.CommentTime = data.CommentTime;
                db.SaveChanges();
                return Ok("Update Success");
            }

            return BadRequest("Unable to Insert and Update");

        }

        public class GetRequest
        {
            public int MerchandiseId;
            public int TransactId;
            public string Comment;
        }


        [HttpPost]
        [Route("api/SellerGetAllTransacts")]
        public IHttpActionResult SellerGetAllTransacts()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
 
             
            //var a = new BookSearch();
            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return BadRequest("Not Login");
            }

            var sellerId = SellerSession.GetSellerIdFromSession(int.Parse(session.Value));
            if (sellerId < 0)
            {
                return BadRequest("Not Login");
            }
            
            
            Transact[] transacts = TransactService.SellerGetAllTransacts(sellerId);
            if (transacts.Length == 0)
            {
                return BadRequest("No Transacts Found");
            }
            return Ok(transacts);
        }
        
        
        [HttpPost]
        [Route("api/CustomerGetAllTransacts")]
        public IHttpActionResult CustomerGetAllTransacts()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
 
             
            //var a = new BookSearch();
            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return BadRequest("Not Login");
            }

            var customerId = CustomerSession.GetCustomerIdFromSession(int.Parse(session.Value));
            if (customerId < 0)
            {
                return BadRequest("Not Login");
            }
            
            
            Transact[] transacts = TransactService.CustomerGetAllTransacts(customerId);
            if (transacts.Length == 0)
            {
                return BadRequest("No Transacts Found");
            }
            return Ok(transacts);
        }
        

        [HttpPost]
        [Route("api/GetAllComments")]
        public IHttpActionResult GetAllComments(GetRequest data)
        {
            TransactService.CommentInfo[] comments = TransactService.GetCommentsOfMerchandise(data.MerchandiseId);
            if (comments.Length == 0)
            {
                return BadRequest("No Comments Found");
            }
            return Ok(comments);
        }
        



        //添加评论接口
        [HttpPost]
        [Route("api/Comment")]
        public IHttpActionResult UpdateComment(GetRequest data)
        {
            var transact = db.Transacts.FirstOrDefault(t => t.TransactId == data.TransactId);

            if (transact == null)
            {
                return BadRequest("No Such Transact");
            }

            transact.Comment = data.Comment;
            transact.CommentTime = DateTime.Now;
            db.SaveChanges();
            return Ok();
        }

        //get ok
        [HttpPost]
        [Route("api/GetTransact")]
        public IHttpActionResult GetTransact(GetRequest data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transact = db.Transacts.Find(data.TransactId);
            if (transact == null)
            {
                return NotFound();
            }

            return Ok(transact);
        }

        [HttpPost]
        [Route("api/GetLogistics")]
        public IHttpActionResult GetLogistics(GetRequest data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
 
             
            //var a = new BookSearch();
            TransactAddressInfo[] transactAddressed = TransactAddress.GetTransactAddress(data.TransactId);
            if (transactAddressed.Length == 0)
            {
                return BadRequest("No Transact Found");
            }
            return Ok(transactAddressed[0]);
        }
    }
}