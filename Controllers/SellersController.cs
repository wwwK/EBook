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
using NETCore.Encrypt;
using EBook.Service;

namespace EBook.Controllers
{
    public class SellerController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        
        public class RegisterData
        {
            public readonly Seller SellerData;
            public readonly string ValidateCode;

            public RegisterData(Seller sellerData, string validateCode)
            {
                SellerData = sellerData;
                ValidateCode = validateCode;
            }
        }

        [HttpPost]
        [Route("api/Seller/")]
        public IHttpActionResult UpdateUser(RegisterData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tmpResult = Service.SellerEmailSend.CheckVerifyCode(data.SellerData.SellerEmail, data.ValidateCode);
            if (tmpResult != 0)
            {
                switch (tmpResult)
                {
                    case -1:
                        tmpResult = Service.SellerSmsSend.CheckVerifyCode(data.SellerData.SellerPhone, data.ValidateCode);
                        if (tmpResult != 0)
                        {
                            switch (tmpResult)
                            {
                                case -1:
                                    return BadRequest("请先点击发送验证码！");
                                case -2:
                                    return BadRequest("验证码错误，请输入正确的验证码！");
                                case -3:
                                    return BadRequest("请重新发送验证码！");
                            }
                        }

                        break;
                    case -2:
                        return BadRequest("验证码错误，请输入正确的验证码！");
                    case -3:
                        return BadRequest("请重新发送验证码！");
                }
            }
//            "Password": "123456",
//            "ShopName": "interesting",
//            "CreditLevel": 10,
//            "ShopDescription": "mmp",
//            "AvatarPath": "mmp",
//            "DefaultSellerAddressIndex": 0,
//            "SellerEmail": "631102050@qq.com",
//            "SellerPhone": "13761491703"

            Seller seller = new Seller()
            {
                Password = EncryptProvider.Md5(data.SellerData.Password),
                ShopName = data.SellerData.ShopName,
                CreditLevel = 5,
                ShopDescription = data.SellerData.ShopDescription,
                SellerEmail = data.SellerData.SellerEmail,
                SellerPhone = data.SellerData.SellerPhone,
                AvatarPath = "seller_avatar",
                DefaultSellerAddressIndex = 0
            };





            var inserted = db.Sellers.Add(seller);

            db.SaveChanges();

            var cookie = new HttpCookie("sessionId")
            {
                Value = Service.CustomerSession.SetSessionId(inserted.SellerId).ToString(),
                Expires = DateTime.Now.AddHours(1)
            };

            HttpContext.Current.Response.Cookies.Add(cookie);


            return Ok();
        }

        public class GetRequest
        {
            public int SellerId;
        }

        [HttpPost]
        [Route("api/GetSeller")]
        public IHttpActionResult GetSeller()
        {
            //maybe false
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return BadRequest("请先登录！");
            }
            
            int sellerId = SellerSession.GetSellerIdFromSession(int.Parse(session.Value));
            if (sellerId < 0)
            {
                return BadRequest("请先登录！");
            }

            var seller = db.Sellers.Find(sellerId);
            if (seller == null)
            {
                return NotFound();
            }

            return Ok(seller);
        }

        public class UpdateInfo
        {
            public readonly string ShopName;
            public readonly int CreditLevel;
            public readonly string ShopDescription;
            public readonly int DefaultSellerAddressIndex;

            public UpdateInfo(string shopName, int creditLevel, string shopDescription, int defaultSellerAddressIndex)
            {
                ShopName = shopName;
                CreditLevel = creditLevel;
                ShopDescription = shopDescription;
                DefaultSellerAddressIndex = defaultSellerAddressIndex;
            }
        }
        
        
        [HttpPost]
        [Route("api/UpdateSeller")]
        public IHttpActionResult UpdateSeller(UpdateInfo data)
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

            int sellerId = SellerSession.GetSellerIdFromSession(int.Parse(session.Value));
            if (sellerId < 0)
            {
                return BadRequest("请先登录！");
            }

            var updateseller = db.Sellers.FirstOrDefault(s => s.SellerId == sellerId);
            if (updateseller != null)
            {
                updateseller.ShopName = data.ShopName;
                updateseller.CreditLevel = data.CreditLevel;
                updateseller.ShopDescription = data.ShopDescription;
                updateseller.DefaultSellerAddressIndex = data.DefaultSellerAddressIndex;
                db.SaveChanges();
                return Ok("修改资料成功！");
            }

            return BadRequest("请重新修改店铺信息！");
        }
    }
}