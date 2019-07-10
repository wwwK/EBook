using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EBook.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using BCrypt.Net;
using System.Web.SessionState;
using EBook.Service;


namespace EBook.Controllers
{
    public class CollectController : ApiController
    {
        private readonly OracleDbContext _db = new OracleDbContext();


        //insert update
        [HttpPost]
        [Route("api/Collect/")]
        public IHttpActionResult InsertCollect(Collect data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return BadRequest("请先登录！");
            }
            
            int customerId = CustomerSession.GetCustomerIdFromSession(int.Parse(session.Value));
            if (customerId < 0)
            {
                return BadRequest("请先登录！");
            }
            if (_db.Collects.Find(customerId,data.MerchandiseId) == null)
            {
                Collect collect = new Collect
                {
                    CustomerId = customerId,
                    MerchandiseId = data.MerchandiseId,
                    CollectTime = DateTime.Now,
                };
                
                _db.Collects.Add(collect);
                _db.SaveChanges();

                return Ok("收藏成功！");
            }

            var updateCollect = _db.Collects.FirstOrDefault(c => c.CustomerId == customerId && c.MerchandiseId == data.MerchandiseId);
            if (updateCollect != null)
            {
                updateCollect.CollectTime = DateTime.Now;
                updateCollect.IsValid = data.IsValid;
                _db.SaveChanges();
                return Ok("更新收藏成功！");
            }

            return BadRequest("请重新收藏！");

        }

/*
        public class GetRequest
        {
            public int CustomerId;
            public int MerchandiseId;
        }

        [HttpGet]
        [Route("api/Collect/1")]
        public IHttpActionResult GetCollect(GetRequest data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var collect = db.Collects.Find(customerId,data.MerchandiseId);
            if (collect == null)
            {
                return NotFound();
            }

            return Ok(collect);
        }*/
    }
}