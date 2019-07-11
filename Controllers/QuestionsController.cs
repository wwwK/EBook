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
        private readonly OracleDbContext _db = new OracleDbContext();


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
                return BadRequest("请先登录！");
            }

            int customerId = CustomerSession.GetCustomerIdFromSession(int.Parse(session.Value));
            if (customerId < 0)
            {
                return BadRequest("请先登录！");
            }

            if (_db.Questions.Find(data.QuestionId) == null)
            {
                Question question = new Question    
                {
                    CustomerId = customerId,
                    AboutMerchandiseId = data.AboutMerchandiseId,
                    SubmitTime = DateTime.Now,
                    Content = data.Content,
                };


                _db.Questions.Add(question);
                _db.SaveChanges();

                return Ok("提问成功！");
            }

            var updateQuestion = _db.Questions.FirstOrDefault(q => q.QuestionId == data.QuestionId);
            if (updateQuestion != null)
            {
                updateQuestion.SubmitTime = data.SubmitTime;
                updateQuestion.Content = data.Content;
                updateQuestion.IsValid = data.IsValid;
                _db.SaveChanges();
                return Ok("更新问题成功！");
            }

            return BadRequest("请重新更新问题！");
        }


        public class GetRequest
        {
            public readonly int QuestionId;

            public GetRequest(int questionId)
            {
                QuestionId = questionId;
            }
        }
        
        //get ok
        [HttpPost]
        [Route("api/GetQuestion")]
        public IHttpActionResult GetQuestion(GetRequest data)
        {
            var question = _db.Questions.Find(data.QuestionId);
            if (question == null)
            {
                return BadRequest();
            }

            return Ok(question);
        }
    }
}