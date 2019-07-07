using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc.Html;
using EBook.Models;
using BCrypt.Net;
using EBook.Service;

namespace EBook.Controllers
{
    public class AuthController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        public class RegisterData
        {
            public string Name;
            public string Email;
            public string Password;
        }
        
        
        public class LoginData
        {
            [EmailAddress]
            public string Email;
            public string Password;
        }

        [HttpPost]
        [Route("api/sendMail")]
        public IHttpActionResult SendMail(LoginData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Email Invalid.");
            }
            
            var result = from customer in db.Customers
                where customer.Email == data.Email
                select customer;

            if (result.Any())
            {
                // 邮箱已存在
                return BadRequest("Email exist");
            }
            EBook.Service.EmailSend.SendVerifyCode(data.Email);
            return Ok();

//            HttpCookie cookie = new HttpCookie("user_cookie")
//            {
//                Value = data.Email,
//                Expires = DateTime.Now.AddHours(1)
//            };
//            HttpContext.Current.Response.Cookies.Add(cookie);
//            HttpContext.Current.Session["id"] = "data.Email";
//            
//            HttpContext.Current.Session["data.password"] = data.Email;

//            if (HttpContext.Current.Session == null)
//            {
//                Console.WriteLine("failed");
//                Console.WriteLine("1");
//                HttpCookie cookie = new HttpCookie("id")
//                {
//                    Value = data.Email,
//                    Expires = DateTime.Now.AddHours(1)
//                };
//                HttpContext.Current.Response.Cookies.Add(cookie);
//                
//                HttpContext.Current.Session.Add("id",data.Email);
//            }
//            else
//            {
//                Console.WriteLine("success");
//            }
        }

        [HttpPost]
        [Route("api/login")]
        public IHttpActionResult Login(LoginData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = from customer in db.Customers
                where customer.Email == data.Email
                select customer;

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
                Value = Service.Session.SetSessionId(result.First().CustomerId).ToString(),
                Expires = DateTime.Now.AddHours(1)
            };
            
            HttpContext.Current.Response.Cookies.Add(cookie);

            // var session = HttpContext.Current.Request.Cookies.Get("sessionId");

            return Ok();
        }

    }
}