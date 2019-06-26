using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using EBook.Models;

namespace EBook.Controllers
{
    public class CustomerController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();
            
        [HttpPost]
        [Route("api/Customer/")]
        public async Task<IHttpActionResult> UpdateUser(Customer data)
        {

             
            
          
            db.Customers.Add(data);
            
            
            await db.SaveChangesAsync();
            
          
            
            
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