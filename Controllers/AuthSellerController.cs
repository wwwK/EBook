
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;
using System.Web.Mvc.Html;
using EBook.Models;
using BCrypt.Net;
using EBook.Service;
using NETCore.Encrypt;

namespace EBook.Controllers
{
    public class AuthSellerController:ApiController
    {
        private readonly OracleDbContext _db = new OracleDbContext();

        public class LoginData
        {
            [EmailAddress] public readonly string Email;

            public readonly string Password;
            //注册=0，修改资料=1
            public readonly int EmailStatus;

            public readonly string Phone;
            public readonly string ValidateCode;

            public LoginData(string email, string password, int emailStatus, string phone, string validateCode)
            {
                Email = email;
                Password = password;
                EmailStatus = emailStatus;
                Phone = phone;
                ValidateCode = validateCode;
            }
        }
        

        
        
        
        [HttpPost]
        [Route("api/SellerSendMail")]
        public IHttpActionResult SendMail(LoginData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("请输入正确的邮件格式！");
            }

            var result = from seller in _db.Sellers
                where seller.SellerEmail == data.Email
                select seller;

            // 邮箱已存在
            if (result.Any() && data.EmailStatus == 0)
            {
                return BadRequest("邮箱已经被注册了");
            }

            EBook.Service.SellerEmailSend.SendVerifyCode(data.Email);
            return Ok();
        }
        
        [HttpPost]
        [Route("api/SellerMailChangePassword")]
        public IHttpActionResult SellerMailChangePassword(LoginData data)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            
            var tmpResult = Service.SellerEmailSend.CheckVerifyCode(data.Email, data.ValidateCode);
            if (tmpResult != 0)
            {
                switch (tmpResult)
                {
                    case -1:
                        return BadRequest("请先点击发送验证码！");
                    case -2:
                        return BadRequest("请输入正确的验证码！");
                    case -3:
                        return BadRequest("请重新发送验证码！");
                }
            }

      
            var updatedSeller = _db.Sellers.FirstOrDefault(b => b.SellerEmail == data.Email);
            if (updatedSeller != null)
            {
                updatedSeller.Password = EncryptProvider.Md5(data.Password);
                _db.SaveChanges();
                return Ok("密码更改成功！");
            }
            else
            {
                return NotFound();
            }
            
        }

        [HttpPost]
        [Route("api/SellerLogin")]
        public IHttpActionResult SellerLogin(LoginData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = from seller in _db.Sellers
                where seller.SellerEmail == data.Email
                select seller;

            if (!result.Any())
            {

                 result = from seller in _db.Sellers
                    where seller.SellerPhone == data.Phone
                    select seller;
                 if (!result.Any())
                 {
                     return NotFound(); 
                 }
                 
            }
            var hashed = EncryptProvider.Md5(data.Password);
            if (result.First().Password != hashed)
            {
                return BadRequest("密码不正确！");
            }
            
            var cookie = new HttpCookie("sessionId")
            {
                Value = SellerSession.SetSessionId(result.First().SellerId).ToString(),
                Expires = DateTime.Now.AddHours(1)
            };
            
            
            HttpContext.Current.Response.Cookies.Add(cookie);
            

            return Ok();
        }
        
        
        
        
        [HttpPost]
        [Route("api/SellerLogout")]
        public IHttpActionResult SellerLogout()
        {
            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return Ok();
            }



            SellerSession.RemoveSellerIdFromSession(int.Parse(session.Value));
           
            HttpContext.Current.Response.Cookies.Remove("sessionId");
            
            return Ok();
        }


    }
}