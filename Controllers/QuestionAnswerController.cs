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
    public class QuestionAnswerController :ApiController
    {
        private OracleDbContext _db = new OracleDbContext();
        
        public class MerchandiseRequest
        {
            public readonly int MerchandiseId;

            public MerchandiseRequest(int merchandiseId)
            {
                MerchandiseId = merchandiseId;
            }
        }
        
        // get Question and Answer by merchandise
        [HttpPost]
        [Route("api/GetQuestionFromMerchandise")]
        public IHttpActionResult GetQuestionFromMerchandise(MerchandiseRequest merchandiseData)
        {
            var result = Service.QuestionAnswerService.GetQuestionsFromMerchandise(merchandiseData.MerchandiseId);
            if (result.Length == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }
        
        public class QuestionRequest
        {
            public readonly int QuestionId;

            public QuestionRequest(int questionId)
            {
                QuestionId = questionId;
            }
        }
        
        
        [HttpPost]
        [Route("api/GetAnswerFromQuestion")]
        public IHttpActionResult GetAnswerFromQuestion(QuestionRequest questionData)
        {
            var result = Service.QuestionAnswerService.GetAnswersFromQuestion(questionData.QuestionId);
            if (result.Length == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}