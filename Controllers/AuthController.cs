using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EBook.Models;
using BCrypt.Net;
using EBook.Service;
using NETCore.Encrypt;

namespace EBook.Controllers
{
    public class AuthController : ApiController
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
        [Route("api/SendMail")]
        public IHttpActionResult SendMail(LoginData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Email Invalid.");
            }

            var result = from customer in db.Customers
                where customer.Email == data.Email
                select customer;

            // 邮箱已存在
            if (result.Any() && data.EmailStatus == 0)
            {
                return BadRequest("Email exist");
            }

            EBook.Service.EmailSend.SendVerifyCode(data.Email);
            return Ok();
        }
        
        [HttpPost]
        [Route("api/MailChangePassword")]
        public IHttpActionResult MailChangePassword(LoginData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var updatedCustomer = db.Customers.FirstOrDefault(b => b.Email == data.Email);
            if (updatedCustomer != null)
            {
                updatedCustomer.Password = EncryptProvider.Md5(data.Password);
                db.SaveChanges();
                return Ok("Update Success");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("api/Login")]
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

            var hashed = EncryptProvider.Md5(data.Password);

            Console.WriteLine(result.First().Password);
            Console.WriteLine(hashed);
            if (result.First().Password != hashed)
            {
                return BadRequest("Password incorrect.");
            }

            var cookie = new HttpCookie("sessionId")
            {
                Value = CustomerSession.SetSessionId(result.First().CustomerId).ToString(),
                Expires = DateTime.Now.AddHours(1)
            };


            HttpContext.Current.Response.Cookies.Add(cookie);


            return Ok();
        }


        [HttpPost]
        [Route("api/Logout")]
        public IHttpActionResult Logout()
        {
            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return Ok();
            }

            CustomerSession.RemoveCustomerIdFromSession(int.Parse(session.Value));
            HttpContext.Current.Response.Cookies.Remove("sessionId");

            return Ok();
        }
    }
}