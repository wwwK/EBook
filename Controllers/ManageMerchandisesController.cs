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
    public class ManageMerchandisesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [HttpPost]
        [Route("api/ManageSelectMerchandise")]
        public IHttpActionResult ManageSelectMerchandise()
        {
            Merchandise [] tableMerchandises = db.Merchandises.ToArray();
            IEnumerable<Merchandise>selectTableMerchandises =
                from merchandise in tableMerchandises
                select merchandise;
            return Ok(selectTableMerchandises.ToArray());

        }
        
        
        [HttpPost]
        [Route("api/ManageInsertMerchandise")]
        public IHttpActionResult ManageInsertMerchandise(Merchandise merchandise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Merchandises.Add(merchandise);
            db.SaveChanges();

            return Ok("Insert Success");
        }
        
        [HttpPost]
        [Route("api/ManageUpdateMerchandise")]
        public IHttpActionResult ManageUpdateMerchandise(Merchandise merchandise)
        {
            Merchandise updatedMerchandise = db.Merchandises.FirstOrDefault(m => m.MerchandiseId == merchandise.MerchandiseId);
            if (updatedMerchandise != null)
            {
                updatedMerchandise = merchandise;
                db.SaveChanges();
                return Ok("更新成功！");
            }

            return BadRequest("请重新操作！");
        }
        
    }

}