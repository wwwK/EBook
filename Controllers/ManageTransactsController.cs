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
    public class ManageTransactsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [HttpPost]
        [Route("api/ManageSelectTransact")]
        public IHttpActionResult ManageSelectTransact()
        {
            Transact [] tableTransacts = db.Transacts.ToArray();
            IEnumerable<Transact>selectTableTransacts =
                from transact in tableTransacts
                select transact;
            return Ok(selectTableTransacts.ToArray());

        }
        
        
        [HttpPost]
        [Route("api/ManageInsertTransact")]
        public IHttpActionResult ManageInsertTransact(Transact transact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Transacts.Add(transact);
            db.SaveChanges();

            return Ok("Insert Success");
        }
        
        [HttpPost]
        [Route("api/ManageUpdateTransact")]
        public IHttpActionResult ManageUpdateTransact(Transact transact)
        {
            Transact updatedTransact = db.Transacts.FirstOrDefault(a => a.TransactId == transact.TransactId);
            if (updatedTransact != null)
            {
                updatedTransact.Status = 0;
                db.SaveChanges();
                return Ok("更新成功！");
            }

            return BadRequest("请重新操作！");
        }
        
    }
}