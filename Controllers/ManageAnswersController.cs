using System.Threading.Tasks;
using System.Web;
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
    public class ManageAnswersController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [HttpPost]
        [Route("api/ManageSelectAnswer")]
        public IHttpActionResult ManageSelectAnswer()
        {
            Answer[] tableAnswers = db.Answers.ToArray();
            IEnumerable<Answer> selectTableAnswers =
                from answer in tableAnswers
                select answer;
            return Ok(selectTableAnswers.ToArray());
        }


        [HttpPost]
        [Route("api/ManageInsertAnswer")]
        public IHttpActionResult ManageInsertAnswer(Answer answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Answers.Add(answer);
            db.SaveChanges();

            return Ok("Insert Success");
        }


        [HttpPost]
        [Route("api/ManageUpdateAnswer")]
        public IHttpActionResult ManageUpdateAnswer(Answer answer)
        {
            Answer updatedAnswer = db.Answers.FirstOrDefault(a => a.AnswerId == answer.AnswerId);
            if (updatedAnswer != null)
            {
                updatedAnswer.IsValid = 0;
                db.SaveChanges();
                return Ok("更新成功！");
            }

            return BadRequest("请重新操作！");
        }
    }
}