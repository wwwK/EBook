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
        public async Task<IHttpActionResult> InsertAnswer(Answer data)
        {
            Answer answer = new Answer
            {
                AnswerId = data.AnswerId,
                CustomerId = data.CustomerId,
                QuestionAnsweredId = data.QuestionAnsweredId,
                SubmitTime = data.SubmitTime,
                Content = data.Content,
            };


            db.Answers.Add(answer);
            

            await db.SaveChangesAsync();
            

            return Ok();
        }


        public class GetRequest
        {
            public int AnswerId;
        }

        [HttpGet]
        [Route("api/Answer/1")]
        public async Task<IHttpActionResult> GetAnswer(GetRequest data)
        {
            var answer = await db.Answers.FindAsync(data.AnswerId);
            if (answer == null)
            {
                throw new HttpException(404, "User not found");
            }

            return Ok(answer);
        }
    }
}