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
    public class QuestionController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        [HttpPost]
        [Route("api/Question/")]
        public IHttpActionResult InsertQuestion(Question data)
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

            if (db.Questions.Find(data.QuestionId) == null)
            {
                Question question = new Question    
                {
                    QuestionId = data.QuestionId,
                    CustomerId = customerId,
                    AboutMerchandiseId = data.AboutMerchandiseId,
                    SubmitTime = data.SubmitTime,
                    Content = data.Content,
                };


                db.Questions.Add(question);
                db.SaveChanges();

                return Ok("Insert Success");
            }

            var updatequestion = db.Questions.FirstOrDefault(q => q.QuestionId == data.QuestionId);
            if (updatequestion != null)
            {
                updatequestion.SubmitTime = data.SubmitTime;
                updatequestion.Content = data.Content;
                updatequestion.IsValid = data.IsValid;
                db.SaveChanges();
                return Ok("Update Success");
            }

            return BadRequest("Unable to Insert and Update");
        }


        public class GetRequest
        {
            public int QuestionId;
        }
        
        //get ok
        [HttpPost]
        [Route("api/GetQuestion")]
        public IHttpActionResult GetQuestion(GetRequest data)
        {
            var question = db.Questions.Find(data.QuestionId);
            if (question == null)
            {
                return BadRequest();
            }

            return Ok(question);
        }
    }
}