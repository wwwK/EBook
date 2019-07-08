using System;
using System.Linq;
using EBook.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace EBook.Service
{
    public static class QuestionAnswerService
    {

        
        
        private static OracleDbContext db = new OracleDbContext();

        public static Question[] GetQuestionsFromMerchandise(int merchandiseId)
        {
            var merchandiseArray = db.Merchandises.ToArray();
            var questionArray = db.Questions.ToArray();
            var results =
                from question in db.Questions
                join merchandise in db.Merchandises on question.AboutMerchandiseId equals merchandise.MerchandiseId
                where merchandise.MerchandiseId == merchandiseId
                select question;

            return results.ToArray();
        }

        public static Answer[] GetAnswersFromQuestion(int questionId)
        {
            var results =
                from answer in db.Answers
                where answer.QuestionAnsweredId == questionId
                select answer;

            return results.ToArray();
        }
    }
}