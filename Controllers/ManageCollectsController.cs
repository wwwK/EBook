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
    public class ManageCollectsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [HttpPost]
        [Route("api/ManageSelectCollect")]
        public IHttpActionResult ManageSelectCollect()
        {
            Collect [] tableCollects = db.Collects.ToArray();
            IEnumerable<Collect>selectTableCollects =
                from collect in tableCollects
                select collect;
            return Ok(selectTableCollects.ToArray());

        }
        
        
        [HttpPost]
        [Route("api/ManageInsertCollect")]
        public IHttpActionResult ManageInsertCollect(Collect collect)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Collects.Add(collect);
            db.SaveChanges();

            return Ok("Insert Success");
        }
        
        [HttpPost]
        [Route("api/ManageUpdateCollect")]
        public IHttpActionResult ManageUpdateCollect(Collect collect)
        {
            Collect updatedCollect = db.Collects.FirstOrDefault(c => c.CustomerId == collect.CustomerId && c.MerchandiseId == collect.MerchandiseId);
            if (updatedCollect != null)
            {
                updatedCollect = collect;
                db.SaveChanges();
                return Ok("更新成功！");
            }

            return BadRequest("请重新操作！");
        }
        
    }
}