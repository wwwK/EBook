using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc.Html;
using EBook.Models;

namespace EBook.Controllers
{
    public class FileController: ApiController
    {
        private readonly OracleDbContext _db = new OracleDbContext();

        private class UploadTokenResponse
        {
            public string Token;
            public string Key;
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
            
            var result = (from customer in _db.Customers
                where customer.CustomerId == userId
                select customer).First();

            result.AvatarPath = fileName;

            _db.SaveChanges();
            
            var token = Service.FileUploadAuthorize.GenerateUploadToken(fileName);
    
            return Ok(new UploadTokenResponse()
            {
                Token = token,
                Key = fileName
            });
        }
         
    }
}