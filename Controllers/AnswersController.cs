using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using EBook.Models;

namespace EBook.Controllers
{
    public class AnswersController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/Answers
        public IQueryable<Answer> GetAnswers()
        {
            return db.Answers;
        }

        // GET: api/Answers/5
        [ResponseType(typeof(Answer))]
        public async Task<IHttpActionResult> GetAnswer(int id)
        {
            Answer answer = await db.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            return Ok(answer);
        }

        // PUT: api/Answers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAnswer(int id, Answer answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != answer.AnswerId)
            {
                return BadRequest();
            }

            db.Entry(answer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Answers
        [ResponseType(typeof(Answer))]
        public async Task<IHttpActionResult> PostAnswer(Answer answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Answers.Add(answer);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = answer.AnswerId }, answer);
        }

        // DELETE: api/Answers/5
        [ResponseType(typeof(Answer))]
        public async Task<IHttpActionResult> DeleteAnswer(int id)
        {
            Answer answer = await db.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            db.Answers.Remove(answer);
            await db.SaveChangesAsync();

            return Ok(answer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnswerExists(int id)
        {
            return db.Answers.Count(e => e.AnswerId == id) > 0;
        }
    }
}