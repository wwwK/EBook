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
    public class VipMemberController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        public class VipRequest
        {
            public string ShopName;
            public double DiscountRatio;
            public DateTime ValidThrough;
        }
        

        //insert update
        [HttpPost]
        [Route("api/VipMember/")]
        public IHttpActionResult InsertVipMember(VipRequest data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return BadRequest("Not Login");
            }

            int customerId = CustomerSession.GetCustomerIdFromSession(int.Parse(session.Value));
            if (customerId < 0)
            {
                return BadRequest("Not Login");
            }

            var sellerId = ShopNameService.GetSellerIdByShopName(data.ShopName);

            if (db.VipMembers.Find(customerId,sellerId) == null)
            {
                VipMember vipMember = new VipMember
                {
                    CustomerId = customerId,
                    SellerId = sellerId,
                    DiscountRatio = data.DiscountRatio,
                    ValidThrough = data.ValidThrough,
                };


                db.VipMembers.Add(vipMember);
                db.SaveChanges();

                return Ok("Insert Success");
            }

            var updatevipmember = db.VipMembers.FirstOrDefault(v => v.CustomerId == customerId && v.SellerId == sellerId);
            if (updatevipmember != null)
            {
                updatevipmember.DiscountRatio = data.DiscountRatio;
                updatevipmember.ValidThrough = data.ValidThrough;
                db.SaveChanges();
                return Ok("Update Success");
            }

            return BadRequest("Unable to Insert and Update");

        }

        public class GetRequest
        {
            public int CustomerId;
            public int SellerId;
        }

        /*[HttpGet]
        [Route("api/VipMember/1")]
        public IHttpActionResult GetVipMember(GetRequest data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var vipMember = db.VipMembers.Find(data.CustomerId,data.SellerId);
            if (vipMember == null)
            {
                return NotFound();
            }

            return Ok(vipMember);
        }*/
    }
}