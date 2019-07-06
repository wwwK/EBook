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
        public async Task<IHttpActionResult> InsertCustomerAddress(CustomerAddress data)
        {
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
            

            await db.SaveChangesAsync();
            

            return Ok();
        }

        public class GetRequest
        {
            public int AddressIndex;
        }

        [HttpGet]
        [Route("api/CustomerAddress/1")]
        public async Task<IHttpActionResult> GetCustomerAddress(GetRequest data)
        {
            var address = await db.CustomerAddresses.FindAsync(data.AddressIndex);
            if (address == null)
            {
                throw new HttpException(404, "User not found");
            }

            return Ok(address);
        }
    }
}