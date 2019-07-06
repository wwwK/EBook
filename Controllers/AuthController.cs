using System;

using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EBook.Models;
using BCrypt.Net;

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
            public string Email;
            public string Password;
        }

        [HttpPost]
        [Route("api/register")]
        public  string register(LoginData data)
        {
            
//            HttpCookie cookie = new HttpCookie("user_cookie")
//            {
//                Value = data.Email,
//                Expires = DateTime.Now.AddHours(1)
//            };
//            HttpContext.Current.Response.Cookies.Add(cookie);
//            HttpContext.Current.Session["id"] = "data.Email";
            
//           HttpContext.Current.Session["data.password"] = data.Email;

            if (HttpContext.Current.Session == null)
            {
                Console.WriteLine("failed");
                Console.WriteLine("1");
                HttpCookie cookie = new HttpCookie("id")
                {
                    Value = data.Email,
                    Expires = DateTime.Now.AddHours(1)
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
                
                HttpContext.Current.Session.Add("id",data.Email);
            }
            else
            {
                Console.WriteLine("success");
            }

//            HttpContext.Current.Session["id"] = data.Email;
//            string code = HttpContext.Current.Session["id"].ToString();
            return data.Email;
        }
        
        

     

    }
}