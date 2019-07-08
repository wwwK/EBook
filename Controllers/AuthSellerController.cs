
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
namespace EBook.Controllers
{
    public class AuthSellerController:ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        public class LoginData
        {
            [EmailAddress] public readonly string Email;

            public readonly string Password;


            //注册=0，修改资料=1
            public readonly int EmailStatus;

            public LoginData(string email, string password, int emailStatus)
            {
                Email = email;
                Password = password;
                EmailStatus = emailStatus;
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
            
            var result = from seller in db.Sellers
                where seller.Email == data.Email
                select seller;

            if (!result.Any())
            {

                return NotFound();
            }
            var hashed = BCrypt.Net.BCrypt.HashPassword(data.Password);
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
            
            return Ok();
        }


    }
}