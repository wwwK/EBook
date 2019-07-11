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
        private readonly OracleDbContext _db = new OracleDbContext();


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
                return BadRequest("请先登录！");
            }
            
            int customerId = CustomerSession.GetCustomerIdFromSession(int.Parse(session.Value));
            if (customerId < 0)
            {
                return BadRequest("请先登录！");
            }

            if (_db.CustomerAddresses.Find(data.AddressIndex) == null)
            {
                CustomerAddress address = new CustomerAddress
                {
                    ReceiverName = data.ReceiverName,
                    ReceivePhone = data.ReceivePhone,
                    Province = data.Province,
                    City = data.City,
                    Block = data.Block,
                    DetailAddress = data.DetailAddress,
                    ZipCode = data.ZipCode,
                    CustomerId = customerId, 
                };
                
            
                _db.CustomerAddresses.Add(address);
                
                
            
                _db.SaveChanges();
            
                return Ok("Insert Success");
            }
            var updateCustomerAddress = _db.CustomerAddresses.FirstOrDefault(ca =>ca.AddressIndex == data.AddressIndex);
            if (updateCustomerAddress == null) return BadRequest("Unable to Insert and Update");
            updateCustomerAddress.ReceiverName = data.ReceiverName;
            updateCustomerAddress.ReceivePhone = data.ReceivePhone;
            updateCustomerAddress.Province = data.Province;
            updateCustomerAddress.City = data.City;
            updateCustomerAddress.Block = data.Block;
            updateCustomerAddress.DetailAddress = data.DetailAddress;
            updateCustomerAddress.ZipCode = data.ZipCode;
            _db.SaveChanges();
            return Ok("Update Success");

        }

        [HttpPost]
        [Route("api/SellerGetAllTransactsDestinationAddress")]
        public IHttpActionResult SellerGetAllTransactsSourceAddress()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return BadRequest("请先登录！");
            }

            var sellerId = SellerSession.GetSellerIdFromSession(int.Parse(session.Value));
            if (sellerId < 0)
            {
                return BadRequest("请先登录");
            }

            DestinationAddressInfo[] destinationAddressInfos =
                TransactAddress.SellerGetAllTransactsDestinationAddressInfos(sellerId);
            if (destinationAddressInfos.Length == 0)
            {
                return BadRequest("找不到对应地址！");
            }

            return Ok(destinationAddressInfos);
        }
        

        public class GetRequest
        {
            public readonly int AddressIndex;

            public GetRequest(int addressIndex)
            {
                AddressIndex = addressIndex;
            }
        }

        [HttpPost]
        [Route("api/GetCustomerAddress")]
        public IHttpActionResult GetCustomerAddress(GetRequest data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var address = _db.CustomerAddresses.Find(data.AddressIndex);
            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }
    }
}