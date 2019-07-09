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
using EBook.Service;

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
            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return BadRequest("Not Login");
            }
            
            int customerId = CustomerSession.GetCustomerIdFromSession(int.Parse(session.Value));
            if (customerId < 0)
            {
                return BadRequest("Not Login");
            }

            if (db.CustomerAddresses.Find(data.AddressIndex) == null)
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
                    CustomerId = customerId, //todo
                };
                
            
                db.CustomerAddresses.Add(address);
            
                db.SaveChanges();
            
                return Ok("Insert Success");
            }
            var updatecustomeraddress = db.CustomerAddresses.FirstOrDefault(ca =>ca.AddressIndex == data.AddressIndex);
            if (updatecustomeraddress != null)
            {
                updatecustomeraddress.ReceiverName = data.ReceiverName;
                updatecustomeraddress.ReceivePhone = data.ReceivePhone;
                updatecustomeraddress.Province = data.Province;
                updatecustomeraddress.City = data.City;
                updatecustomeraddress.Block = data.Block;
                updatecustomeraddress.DetailAddress = data.DetailAddress;
                updatecustomeraddress.ZipCode = data.ZipCode;
                db.SaveChanges();
                return Ok("Update Success");
            }

            return BadRequest("Unable to Insert and Update");

        }

        public class GetRequest
        {
            public int AddressIndex;
        }

        [HttpGet]
        [Route("api/GetCustomerAddress")]
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