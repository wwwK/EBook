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
        private readonly OracleDbContext _db = new OracleDbContext();


        public class LoginData
        {
            [EmailAddress] public readonly string Email;

            public readonly string Password;

            [Phone] public readonly string Phone;


            //注册=0，修改资料=1
            public readonly int EmailStatus;
            
            
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
        [Route("api/SendMail")]
        public IHttpActionResult SendMail(LoginData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("邮件格式不正确，请输入正确的邮件格式！");
            }

            var result = from customer in _db.Customers
                where customer.Email == data.Email
                select customer;

            // 邮箱已存在
            if (result.Any() && data.EmailStatus == 0)
            {
                return BadRequest("您的邮箱已经被注册了！");
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

            var tmpResult = Service.EmailSend.CheckVerifyCode(data.Email, data.ValidateCode);
            if (tmpResult != 0)
            {
                switch (tmpResult)
                {
                    case -1:
                        return BadRequest("请先发送验证码！");
                    case -2:
                        return BadRequest("请输入正确的验证码！");
                    case -3:
                        return BadRequest("请重新验证！");
                }
            }

            var updatedCustomer = _db.Customers.FirstOrDefault(b => b.Email == data.Email);
            if (updatedCustomer != null)
            {
                updatedCustomer.Password = EncryptProvider.Md5(data.Password);
                _db.SaveChanges();
                return Ok("修改密码成功");
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

            var result = from customer in _db.Customers
                where customer.Email == data.Email
                select customer;

            if (!result.Any())
            {
                result = from customer in _db.Customers
                    where customer.PhoneNum == data.Phone
                    select customer;
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
        
        
        public class WeChatRequest
        {
            public string id;
            public string password;
        }

        public class WechatEbookInfo
        {
            public string title;
            public string key;
        }
        
        public class WechatResponse
        {
            public bool ok;
            public WechatEbookInfo[] result;
            public string error;
        }
        
        
        [HttpPost]
        [Route("api/WechatMiniPhoneLogin")]
        public IHttpActionResult WechatMiniPhoneLogin(WeChatRequest data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = _db.Customers.First(customer => customer.PhoneNum == data.id);
            if (user == null)
            {
                return Ok(new WechatResponse()
                {
                    ok = false,
                    error = "Customer not found"
                });
            }

            if (user.Password != EncryptProvider.Md5(data.password))
            {
                return Ok(new WechatResponse()
                {
                    ok = false,
                    error = "Wrong password."
                });
            }

            var result =
                (from transact in _db.Transacts
                where transact.CustomerId == user.CustomerId
                join merchandise in _db.Merchandises on transact.MerchandiseId equals merchandise.MerchandiseId
                join book in _db.Books on merchandise.ISBN equals book.ISBN
                where book.EBookKey != null
                select new WechatEbookInfo()
                {
                    title = book.Title,
                    key = book.EBookKey
                }).ToArray();

            return Ok(new WechatResponse()
            {
                ok = true,
                result = result
            });
        }
       
        [HttpPost]
        [Route("api/WechatMiniEmailLogin")]
        public IHttpActionResult WechatMiniEmailLogin(WeChatRequest data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = _db.Customers.First(customer => customer.Email == data.id);
            if (user == null)
            {
                return Ok(new WechatResponse()
                {
                    ok = false,
                    error = "Customer not found"
                });
            }

            if (user.Password != EncryptProvider.Md5(data.password))
            {
                return Ok(new WechatResponse()
                {
                    ok = false,
                    error = "Wrong password."
                });
            }

            var result =
                (from transact in _db.Transacts
                    where transact.CustomerId == user.CustomerId
                    join merchandise in _db.Merchandises on transact.MerchandiseId equals merchandise.MerchandiseId
                    join book in _db.Books on merchandise.ISBN equals book.ISBN
                    where book.EBookKey != null
                    select new WechatEbookInfo()
                    {
                        title = book.Title,
                        key = book.EBookKey
                    }).ToArray();

            return Ok(new WechatResponse()
            {
                ok = true,
                result = result
            });
        }
    }
}