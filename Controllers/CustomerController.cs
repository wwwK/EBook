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
        public async Task<IHttpActionResult> UpdateUser(RegisterData data)
        {
            var tmpResult = Service.EmailSend.CheckVerifyCode(data.CustomerData.Email, data.ValidateCode);
            if (tmpResult != 0)
            {
                switch (tmpResult)
                {
                    case -1:
                        throw new HttpException(400, "请先发送验证码");
                    case -2:
                        throw new HttpException(400, "验证码错误");
                    case -3:
                        throw new HttpException(400, "验证码失效");
                }
            }

            Customer customer = new Customer()
            {
                FirstName = data.CustomerData.FirstName,
                LastName = data.CustomerData.LastName,
                NickName = data.CustomerData.NickName,
                DefaultAddressIndex = data.CustomerData.DefaultAddressIndex,
                IdCardNum = data.CustomerData.IdCardNum,
                Email = data.CustomerData.Email,
                PhoneNum = data.CustomerData.PhoneNum,
                DateOfBirth = data.CustomerData.DateOfBirth,
                Point = data.CustomerData.Point,
                Password = BCrypt.Net.BCrypt.HashPassword(data.CustomerData.Password)
            };


//                


            var inserted = db.Customers.Add(customer);
            
            await db.SaveChangesAsync();
            
            HttpCookie cookie = new HttpCookie("sessionId")
                {
                    Value = Service.Session.SetSessionId(inserted.CustomerId).ToString(),
                    Expires = DateTime.Now.AddHours(1)
                };
            
            HttpContext.Current.Response.Cookies.Add(cookie);

//                HttpContext.Current.Session.Add("id",data.Email);
//            Console.WriteLine("before" + HttpContext.Current.Session["id"]);


//            HttpContext.Current.Session["id"] = data.Email;
//            Console.WriteLine("after" + HttpContext.Current.Session["id"]);
//
//            string session = HttpContext.Current.Session["id"].ToString();
//            Console.WriteLine("return" + HttpContext.Current.Session["id"]);

            return Ok();
        }


        [HttpGet]
        [Route("api/Customer/{CustomerId}")]
        public async Task<IHttpActionResult> GetUser(int customerId)
        {
            var customer = await db.Customers.FindAsync(customerId);
            if (customer == null)
            {
                throw new HttpException(404, "User not found");
            }

            return Ok(customer);
        }
    }
}