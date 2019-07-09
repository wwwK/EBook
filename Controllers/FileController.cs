using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc.Html;
using EBook.Models;

namespace EBook.Controllers
{
    public class FileController: ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        class UploadTokenResponse
        {
            public string token;
            public string key;
        }
        
        [HttpPost]
        [Route("api/RequestCustomerAvatarUpload")]
        public IHttpActionResult RequestAvatarUpload()
        {
            var cookie = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (cookie == null)
            {
                return BadRequest("Not logged in.");
            }

            var userId = Service.CustomerSession.GetCustomerIdFromSession(int.Parse(cookie.Value));
            if (userId < 0)
            {
                return BadRequest("Not logged in.");
            }

            var fileName = $"{userId}_avatar";
            
            var result = (from customer in db.Customers
                where customer.CustomerId == userId
                select customer).First();

            result.AvatarPath = fileName;

            db.SaveChanges();
            
            var token = Service.FileUploadAuthorize.GenerateUploadToken(fileName);
    
            return Ok(new UploadTokenResponse()
            {
                token = token,
                key = fileName
            });
        }
         
    }
}