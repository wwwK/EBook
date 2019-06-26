using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using EBook.Models;

namespace GeniusBar.Controllers
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
        

        

    }
}