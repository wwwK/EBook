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
    public class CustomerAddressController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        [HttpPost]
        [Route("api/CustomerAddress/")]
        public IHttpActionResult InsertCustomerAddress(CustomerAddress data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            CustomerAddress address = new CustomerAddress
            {
                AddressIndex = data.AddressIndex,
                ReceiverName = data.ReceiverName,
                ReceivePhone = data.ReceivePhone,
                Province = data.Province,
                City = data.City,
                Block = data.Block,
                DetailAddress = data.DetailAddress,
                ZipCode = data.ZipCode,
                CustomerId = data.CustomerId,
                IsDefault = data.IsDefault,
            };
            
            db.CustomerAddresses.Add(address);
            
            db.SaveChanges();
            
            return Ok();
        }

        public class GetRequest
        {
            public int AddressIndex;
        }

        [HttpGet]
        [Route("api/CustomerAddress/1")]
        public IHttpActionResult GetCustomerAddress(GetRequest data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var address = db.CustomerAddresses.Find(data.AddressIndex);
            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }
    }
}