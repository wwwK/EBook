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


namespace EBook.Controllers
{
    public class CustomerController : ApiController, IRequiresSessionState
    {
        private OracleDbContext db = new OracleDbContext();


        public class RegisterData
        {
            public Customer CustomerData;
            public string ValidateCode;
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
                DefaultAddressIndex = data.CustomerData.DefaultAddressIndex,
                IdCardNum = data.CustomerData.IdCardNum,
                Email = data.CustomerData.Email,
                PhoneNum = data.CustomerData.PhoneNum,
                DateOfBirth = data.CustomerData.DateOfBirth,
                Point = data.CustomerData.Point,
                Password = EncryptProvider.Md5(data.CustomerData.Password)
            };

            var inserted = db.Customers.Add(customer);

            db.SaveChanges();

            var cookie = new HttpCookie("sessionId")
            {
                Value = Service.CustomerSession.SetSessionId(inserted.CustomerId).ToString(),
                Expires = DateTime.Now.AddHours(1)
            };

            HttpContext.Current.Response.Cookies.Add(cookie);


            return Ok();
        }


        [HttpGet]
        [Route("api/Customer/{CustomerId}")]
        public IHttpActionResult GetUser(int customerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = db.Customers.Find(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }
    }
}