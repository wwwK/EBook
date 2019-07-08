using System.Threading.Tasks;
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
                    TransactId = data.TransactId,
                    CustomerId = customerId,
                    MerchandiseId = data.MerchandiseId,
                    CreateTime = data.CreateTime,
                    UsedCouponId = data.UsedCouponId,
                    ActualPrice = data.ActualPrice,
                    Status = data.Status,
                    Amount = data.Amount,
                    LogisticTrackNum = data.LogisticTrackNum,
                    Comment = data.Comment,
                    CommentTime = data.CommentTime,
                };


                db.Transacts.Add(transact);
                db.SaveChanges();

                return Ok("Insert Success");
            }

            var updatetransact = db.Transacts.FirstOrDefault(t => t.TransactId == data.TransactId);
            if (updatetransact != null)
            {
                updatetransact.CreateTime = data.CreateTime;
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
            public int SellerId;
            public int TransactId;
            public string Comment;
        }
        
        [HttpPost]
        [Route("api/GetAllComments")]
        public IHttpActionResult GetAllTransacts(GetRequest data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
 
             
            //var a = new BookSearch();
            Transact[] transacts = TransactService.GetAllTransacts(data.SellerId);
            if (transacts.Length == 0)
            {
                return BadRequest("No Transacts Found");
            }
            return Ok(transacts);
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
    }
}