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
        private readonly OracleDbContext _db = new OracleDbContext();


        public class VipRequest
        {
            public readonly string ShopName;
            public readonly double DiscountRatio;
            public DateTime ValidThrough;

            public VipRequest(string shopName, double discountRatio, DateTime validThrough)
            {
                ShopName = shopName;
                DiscountRatio = discountRatio;
                ValidThrough = validThrough;
            }
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
                return BadRequest("请先登录！");
            }

            int customerId = CustomerSession.GetCustomerIdFromSession(int.Parse(session.Value));
            if (customerId < 0)
            {
                return BadRequest("请先登录！");
            }

            var sellerId = ShopNameService.GetSellerIdByShopName(data.ShopName);

            if (_db.VipMembers.Find(customerId,sellerId) == null)
            {
                VipMember vipMember = new VipMember
                {
                    CustomerId = customerId,
                    SellerId = sellerId,
                    DiscountRatio = data.DiscountRatio,
                    ValidThrough = data.ValidThrough,
                };


                _db.VipMembers.Add(vipMember);
                _db.SaveChanges();

                return Ok("你已经成为会员了！");
            }

            var updateVipMember = _db.VipMembers.FirstOrDefault(v => v.CustomerId == customerId && v.SellerId == sellerId);
            if (updateVipMember != null)
            {
                updateVipMember.DiscountRatio = data.DiscountRatio;
                updateVipMember.ValidThrough = data.ValidThrough;
                _db.SaveChanges();
                return Ok("会员延期成功！");
            }

            return BadRequest("会员获取失败，请找系统管理员询问！");

        }

        
        [HttpPost]
        [Route("api/GetVipOfCustomer")]
        public IHttpActionResult GetVipOfCustomer()
        {
            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return BadRequest("请先登录！");
            }

            var currentCustomerId = Service.CustomerSession.GetCustomerIdFromSession(int.Parse(session.Value));
            if (currentCustomerId < 0)
            {
                return BadRequest("请先登录！");
            }
            
            var result = Service.VipCheck.GetVipMemberFromCustomer(currentCustomerId);

            if (result.Length == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpPost]
        [Route("api/GetVipOfSeller")]
        public IHttpActionResult GetVipOfSeller()
        {
            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return BadRequest("请先登录！");
            }

            var currentSellerId = Service.SellerSession.GetSellerIdFromSession(int.Parse(session.Value));
            if (currentSellerId < 0)
            {
                return BadRequest("请先登录！");
            }
            
            var result = Service.VipCheck.GetVipMemberFromSeller(currentSellerId);

            if (result.Length == 0)
            {
                return NotFound();
            }

            return Ok(result);
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