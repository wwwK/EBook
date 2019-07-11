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
        private readonly OracleDbContext _db = new OracleDbContext();


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
                return BadRequest("请先登录！");
            }

            var customerId = CustomerSession.GetCustomerIdFromSession(int.Parse(session.Value));
            if (customerId < 0)
            {
                return BadRequest("请先登录");
            }

            if (_db.Answers.Find(data.AnswerId) == null)
            {
                Answer answer = new Answer    
                {
                    CustomerId = customerId,
                    QuestionAnsweredId = data.QuestionAnsweredId,
                    SubmitTime = DateTime.Now,
                    Content = data.Content,
                };


                _db.Answers.Add(answer);
                _db.SaveChanges();

                return Ok("添加回答成功");
            }

            var updateAnswer = _db.Answers.FirstOrDefault(a => a.AnswerId == data.AnswerId);


            if (updateAnswer == null) return BadRequest("Unable to Insert and Update");
            updateAnswer.SubmitTime = DateTime.Now;
            updateAnswer.Content = data.Content;
            updateAnswer.IsValid = data.IsValid;
            _db.SaveChanges();
            return Ok("更新回答成功！");

        }


        public class GetRequest
        {
            public readonly int AnswerId;

            public GetRequest(int answerId)
            {
                AnswerId = answerId;
            }
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
            var answer = _db.Answers.Find(data.AnswerId);
            if (answer == null)
            {
                return NotFound();
            }

            return Ok(answer);
        }
    }
}