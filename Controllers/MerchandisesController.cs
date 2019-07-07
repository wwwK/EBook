﻿using System.Threading.Tasks;
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


namespace EBook.Controllers
{
    public class MerchandiseController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        [HttpPost]
        [Route("api/Merchandise/")]
        public IHttpActionResult InsertMerchandise(Merchandise data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            Merchandise merchandise = new Merchandise
            {
                MerchandiseId = data.MerchandiseId,
                SellerId = data.SellerId,
                ISBN = data.ISBN,
                Description = data.Description,
                Price = data.Price,
            };

            db.Merchandises.Add(merchandise);
            

            db.SaveChanges();
            
            return Ok();
        }

        public class GetRequest
        {
            public int MerchandiseId;
            public string Comment;
        }

        //添加评论接口
        [HttpPost]
        [Route("api/Comment")]
        public IHttpActionResult UpdateComment(GetRequest data)
        {
            var transact = db.Transacts.FirstOrDefault(m => m.MerchandiseId == data.MerchandiseId);

            if (transact != null)
            {
                transact.Comment = data.Comment;
                return BadRequest("No Such Transact");
            }

            db.SaveChanges();
            return Ok();
        }
        

        [HttpGet]
        [Route("api/Merchandise/1")]
        public IHttpActionResult GetMerchandise(GetRequest data)
        {
            var merchandise = db.Merchandises.Find(data.MerchandiseId);
            if (merchandise == null)
            {
                return NotFound();
            }

            return Ok(merchandise);
        }
    }
}