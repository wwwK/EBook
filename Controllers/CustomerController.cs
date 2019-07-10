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
            public  Customer CustomerData;
            public  string ValidateCode;

//            public RegisterData(Customer customerData, string validateCode)
//            {
//                CustomerData = customerData;
//                ValidateCode = validateCode;
//            }
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
                        return BadRequest("Validate code not sent.");
                    case -2:
                        return BadRequest("Wrong validate code.");
                    case -3:
                        return BadRequest("Validate code expired.");
                }
            }

            Customer customer = new Customer()
            {
                RealName = data.CustomerData.RealName,
                NickName = data.CustomerData.NickName,
                IdCardNum = data.CustomerData.IdCardNum,
                Email = data.CustomerData.Email,
                DateOfBirth = data.CustomerData.DateOfBirth,
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
        public IHttpActionResult GetCustomer(Customer data)
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

            var customer = _db.Customers.Find(data.CustomerId);


            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        public class UpdateInfo
        {
            public string NickName;
            public int DefaultAddressIndex;
            public DateTime DateOfBirth;
            public int Point;
            public int IsValid = 1;
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
                return BadRequest("Not Login");
            }

            int customerId = CustomerSession.GetCustomerIdFromSession(int.Parse(session.Value));
            if (customerId < 0)
            {
                return BadRequest("Not Login");
            }

            var updatecustomer = _db.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (updatecustomer != null)
            {
                updatecustomer.NickName = data.NickName;
                updatecustomer.DefaultAddressIndex = data.DefaultAddressIndex;
                updatecustomer.DateOfBirth = data.DateOfBirth;
                updatecustomer.Point = data.Point;
                updatecustomer.IsValid = data.IsValid;
                _db.SaveChanges();
                return Ok("Update Success");
            }

            return BadRequest("Unable to Insert and Update");
        }
    }
}