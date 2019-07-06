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


namespace EBook.Controllers
{
    public class VipMemberController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        [HttpPost]
        [Route("api/VipMember/")]
        public IHttpActionResult InsertVipMember(VipMember data)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            VipMember vipMember = new VipMember
            {
                CustomerId = data.CustomerId,
                SellerId = data.SellerId,
                DiscountRatio = data.DiscountRatio,
                ValidThrough = data.ValidThrough,
            };

 

            db.VipMembers.Add(vipMember);
            

            db.SaveChanges();
            

            return Ok();
        }

        public class GetRequest
        {
            public int CustomerId;
            public int SellerId;
        }

        [HttpGet]
        [Route("api/VipMember/1")]
        public IHttpActionResult GetVipMember(GetRequest data)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var vipMember = db.VipMembers.Find(data.CustomerId,data.SellerId);
            if (vipMember == null)
            {
                return NotFound();
            }

            return Ok(vipMember);
        }
    }
}