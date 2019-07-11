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
    public class ManageOwnsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [HttpPost]
        [Route("api/ManageSelectOwn")]
        public IHttpActionResult ManageSelectOwn()
        {
            Own [] tableOwns = db.Owns.ToArray();
            IEnumerable<Own>selectTableOwns =
                from own in tableOwns
                select own;
            return Ok(selectTableOwns.ToArray());

        }
        
        
        [HttpPost]
        [Route("api/ManageInsertOwn")]
        public IHttpActionResult ManageInsertOwn(Own own)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Owns.Add(own);
            db.SaveChanges();

            return Ok("Insert Success");
        }
        
        [HttpPost]
        [Route("api/ManageUpdateOwn")]
        public IHttpActionResult ManageUpdateOwn(Own own)
        {
            Own updatedOwn = db.Owns.FirstOrDefault(o => o.CustomerId == own.CustomerId && o.CouponId == own.CouponId);
            if (updatedOwn != null)
            {
                updatedOwn.IsValid = 0;
                db.SaveChanges();
                return Ok("更新成功！");
            }

            return BadRequest("请重新操作！");
        }
        
    }

}