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
            Question question = new Question
            {
                QuestionId = data.QuestionId,
                CustomerId = data.CustomerId,
                AboutMerchandiseId = data.AboutMerchandiseId,
                SubmitTime = data.SubmitTime,
                Content = data.Content,
            };

            db.Questions.Add(question);
            

            db.SaveChanges();
            

            return Ok();
        }


        public class GetRequest
        {
            public int QuestionId;
        }

        [HttpGet]
        [Route("api/Question/1")]
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