using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EBook.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using BCrypt.Net;
using System.Web.SessionState;
using EBook.Service;

namespace EBook.Controllers
{
    public class AnswerController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        //insert update
        [HttpPost]
        [Route("api/Answer/")]
        public IHttpActionResult InsertAnswer(Answer data)
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

            if (db.Answers.Find(data.AnswerId) == null)
            {
                Answer answer = new Answer    
                {
                    AnswerId = data.AnswerId,
                    CustomerId = customerId,
                    QuestionAnsweredId = data.QuestionAnsweredId,
                    SubmitTime = data.SubmitTime,
                    Content = data.Content,
                };


                db.Answers.Add(answer);
                db.SaveChanges();

                return Ok("Insert Success");
            }

            var updateanswer = db.Answers.FirstOrDefault(a => a.AnswerId == data.AnswerId);
            if (updateanswer != null)
            {
                updateanswer.SubmitTime = data.SubmitTime;
                updateanswer.Content = data.Content;
                updateanswer.IsValid = data.IsValid;
                db.SaveChanges();
                return Ok("Update Success");
            }

            return BadRequest("Unable to Insert and Update");
        }


        public class GetRequest
        {
            public int AnswerId;
        }

        //get ok
        [HttpPost]
        [Route("api/GetAnswer")]
        public IHttpActionResult GetAnswer(GetRequest data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var answer = db.Answers.Find(data.AnswerId);
            if (answer == null)
            {
                return NotFound();
            }

            return Ok(answer);
        }
    }
}