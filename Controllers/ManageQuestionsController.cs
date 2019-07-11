using System.Web.Http;
using EBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using BCrypt.Net;
using System.Web.SessionState;
using NETCore.Encrypt;
using EBook.Service;


namespace EBook.Controllers
{
    public class ManageQuestionsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [HttpPost]
        [Route("api/ManageSelectQuestion")]
        public IHttpActionResult ManageSelectQuestion()
        {
            Question [] tableQuestions = db.Questions.ToArray();
            IEnumerable<Question>selectTableQuestions =
                from question in tableQuestions
                select question;
            return Ok(selectTableQuestions.ToArray());

        }
        
        
        [HttpPost]
        [Route("api/ManageInsertQuestion")]
        public IHttpActionResult ManageInsertQuestion(Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Questions.Add(question);
            db.SaveChanges();

            return Ok("Insert Success");
        }
        
        [HttpPost]
        [Route("api/ManageUpdateQuestion")]
        public IHttpActionResult ManageUpdateQuestion(Question question)
        {
            Question updatedQuestion = db.Questions.FirstOrDefault(q => q.QuestionId == question.QuestionId);
            if (updatedQuestion != null)
            {
                updatedQuestion.IsValid = 0;
                db.SaveChanges();
                return Ok("更新成功！");
            }

            return BadRequest("请重新操作！");
        }
        
    }


}