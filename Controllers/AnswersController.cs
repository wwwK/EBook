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


namespace EBook.Controllers
{
    public class AnswerController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        [HttpPost]
        [Route("api/Answer/")]
        public IHttpActionResult InsertAnswer(Answer data)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Answer answer = new Answer
            {
                AnswerId = data.AnswerId,
                CustomerId = data.CustomerId,
                QuestionAnsweredId = data.QuestionAnsweredId,
                SubmitTime = data.SubmitTime,
                Content = data.Content,
            };


            db.Answers.Add(answer);
            

            db.SaveChanges();
            

            return Ok();
        }


        public class GetRequest
        {
            public int AnswerId;
        }

        [HttpGet]
        [Route("api/Answer/1")]
        public IHttpActionResult GetAnswer(GetRequest data)
        {
            if (ModelState.IsValid)
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