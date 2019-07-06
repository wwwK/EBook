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
        public async Task<IHttpActionResult> InsertVipMember(VipMember data)
        {
            VipMember vipMember = new VipMember
            {
                CustomerId = data.CustomerId,
                SellerId = data.SellerId,
                DiscountRatio = data.DiscountRatio,
                ValidThrough = data.ValidThrough,
            };

 

            db.VipMembers.Add(vipMember);
            

            await db.SaveChangesAsync();
            

            return Ok();
        }

        public class GetRequest
        {
            public int CustomerId;
            public int SellerId;
        }

        [HttpGet]
        [Route("api/VipMember/1")]
        public async Task<IHttpActionResult> GetVipMember(GetRequest data)
        {
            var vipMember = await db.VipMembers.FindAsync(data.CustomerId,data.SellerId);
            if (vipMember == null)
            {
                throw new HttpException(404, "User not found");
            }

            return Ok(vipMember);
        }
    }
}