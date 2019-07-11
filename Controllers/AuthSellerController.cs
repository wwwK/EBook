
using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
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
        private static OracleDbContext db = new OracleDbContext();

        public class LoginData
        {
            [EmailAddress] public readonly string Email;

            public readonly string Password;
            //注册=0，修改资料=1
            public readonly int EmailStatus;
            
            
            public string ValidateCode;

            public LoginData(string email, string password, int emailStatus)
            {
                Email = email;
                Password = password;
                EmailStatus = emailStatus;
            }
        }
        

        
        [HttpPost]
        [Route("api/SellerSendMail")]
        public IHttpActionResult SendMail(LoginData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Email Invalid.");
            }

            var result = from seller in db.Sellers
                where seller.SellerEmail == data.Email
                select seller;

            // 邮箱已存在
            if (result.Any() && data.EmailStatus == 0)
            {
                return BadRequest("Email exist");
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
                        return BadRequest("Validate code not sent.");
                    case -2:
                        return BadRequest("Wrong validate code.");
                    case -3:
                        return BadRequest("Validate code expired.");
                }
            }

      
            var updatedSeller = db.Sellers.FirstOrDefault(b => b.SellerEmail == data.Email);
            if (updatedSeller != null)
            {
                updatedSeller.Password = EncryptProvider.Md5(data.Password);
                db.SaveChanges();
                return Ok("Update Success");
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
            
            var result = (from seller in db.Sellers
                where seller.SellerEmail == data.Email
                select seller).ToArray();

            if (result.Length == 0)
            {
                return BadRequest("Fuck you");
            }
            var hashed = EncryptProvider.Md5(data.Password);
            if (result.First().Password != hashed)
            {
                return BadRequest("Password incorrect.");
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
            
            return Ok(HttpContext.Current.Request.Cookies);
        }


    }
}