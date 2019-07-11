using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EBook.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
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
    public class CustomerController : ApiController, IRequiresSessionState
    {
        private readonly OracleDbContext _db = new OracleDbContext();


        public class RegisterData
        {
            public readonly Customer CustomerData;
            public readonly string ValidateCode;

            public RegisterData(Customer customerData, string validateCode)
            {
                CustomerData = customerData;
                ValidateCode = validateCode;
            }
        }

        [HttpPost]
        [Route("api/Customer/")]
        public IHttpActionResult UpdateUser(RegisterData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tmpResult = Service.EmailSend.CheckVerifyCode(data.CustomerData.Email, data.ValidateCode);
            
            
            if (tmpResult != 0)
            {
                switch (tmpResult)
                {
                    case -1:
                        tmpResult = Service.SmsSend.CheckVerifyCode(data.CustomerData.PhoneNum, data.ValidateCode);
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

            var customer = new Customer()
            {
                RealName = data.CustomerData.RealName,
                NickName = data.CustomerData.NickName,
                IdCardNum = data.CustomerData.IdCardNum,
                Email = data.CustomerData.Email,
                DateOfBirth = data.CustomerData.DateOfBirth,
                PhoneNum = data.CustomerData.PhoneNum,
                Password = EncryptProvider.Md5(data.CustomerData.Password),
                Gender = data.CustomerData.Gender,
            };

            var inserted = _db.Customers.Add(customer);

           
                _db.SaveChanges();
           

            var cookie = new HttpCookie("sessionId")
            {
                Value = Service.CustomerSession.SetSessionId(inserted.CustomerId).ToString(),
                Expires = DateTime.Now.AddHours(1)
            };

            HttpContext.Current.Response.Cookies.Add(cookie);


            return Ok();
        }


        [HttpPost]
        [Route("api/GetCustomer")]
        public IHttpActionResult GetCustomer()
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

            var customerId = CustomerSession.GetCustomerIdFromSession(int.Parse(session.Value));
            if (customerId < 0)
            {
                return BadRequest("请先登录！");
            }

            var customer = _db.Customers.Find(customerId);


            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        public class UpdateInfo
        {
            public readonly string NickName;
            public readonly int DefaultAddressIndex;
            public DateTime DateOfBirth;
            public readonly int Point;
            public const int IsValid = 1;

            public UpdateInfo(string nickName, int defaultAddressIndex, DateTime dateOfBirth, int point)
            {
                NickName = nickName;
                DefaultAddressIndex = defaultAddressIndex;
                DateOfBirth = dateOfBirth;
                Point = point;
            }
        }

        [HttpPost]
        [Route("api/UpdateCustomer")]
        public IHttpActionResult UpdateCustomer(UpdateInfo data)
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

            var updateCustomer = _db.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (updateCustomer == null) return BadRequest("请重新更新用户！");
            updateCustomer.NickName = data.NickName;
            updateCustomer.DefaultAddressIndex = data.DefaultAddressIndex;
            updateCustomer.DateOfBirth = data.DateOfBirth;
            updateCustomer.Point = data.Point;
            updateCustomer.IsValid = UpdateInfo.IsValid;
            _db.SaveChanges();
            return Ok("用户数据更新成功！");

        }
    }
}