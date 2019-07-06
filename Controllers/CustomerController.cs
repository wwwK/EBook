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


        [HttpPost]
        [Route("api/Customer/")]
        public async Task<IHttpActionResult> InsertUser(Customer data)
        {
            Customer customer = new Customer()
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                NickName = data.NickName,
                DefaultAddressIndex = data.DefaultAddressIndex,
                IdCardNum = data.IdCardNum,
                Email = data.Email,
                PhoneNum = data.PhoneNum,
                DateOfBirth = data.DateOfBirth,
                Point = data.Point,
                Password = BCrypt.Net.BCrypt.HashPassword(data.Password)
            };


//                var mailService = new Mafly.Mail.Mail();
//                mailService.Send(data.Email, "你的验证码是：7788");


            db.Customers.Add(customer);


            await db.SaveChangesAsync();


            
            
            
            HttpCookie cookie = new HttpCookie("id")
                {
                    Value = data.Email,
                    Expires = DateTime.Now.AddHours(1)
                };
                HttpContext.Current.Response.Cookies.Add(cookie);

               var session = HttpContext.Current.Request.Cookies.Get("SessionId");
                
//                HttpContext.Current.Session.Add("id",data.Email);
//            Console.WriteLine("before" + HttpContext.Current.Session["id"]);


//            HttpContext.Current.Session["id"] = data.Email;
//            Console.WriteLine("after" + HttpContext.Current.Session["id"]);
//
//            string session = HttpContext.Current.Session["id"].ToString();
//            Console.WriteLine("return" + HttpContext.Current.Session["id"]);

            return Ok(session);
        }


        public class GetRequest
        {
            public int CustomerId;
        }

        [HttpGet]
        [Route("api/Customer/1")]
        public async Task<IHttpActionResult> GetCustomer(GetRequest data)
        {
            var customer = await db.Customers.FindAsync(data.CustomerId);
            if (customer == null)
            {
                throw new HttpException(404, "User not found");
            }

            return Ok(customer);
        }
    }
}