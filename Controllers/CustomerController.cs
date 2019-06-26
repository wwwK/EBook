
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Results;
using System.Security.Cryptography;
using EBook.Models;
using MD5 = OracleInternal.Secure.Network.MD5;

namespace EBook.Controllers
{
    public class CustomerController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();
            
        [HttpPost]
        [Route("api/Customer/")]
        public async Task<IHttpActionResult> UpdateUser(Customer data)
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
                Point = data.Point
            };
            

            db.Customers.Add(customer);
            
            
            await db.SaveChangesAsync();
            
          


//            HttpContext.Current.Session["id"] = data.Email;
          
            
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