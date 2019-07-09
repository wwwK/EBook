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
    public class SellerAddressController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        [HttpPost]
        [Route("api/SellerAddress/")]
        public IHttpActionResult InsertCustomerAddress(SellerAddress data)
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
            
            int sellerId = SellerSession.GetSellerIdFromSession(int.Parse(session.Value));
            if (sellerId < 0)
            {
                return BadRequest("Not Login");
            }

            if (db.SellerAddresses.Find(data.AddressIndex) == null)
            {
                SellerAddress address = new SellerAddress
                {
                    AddressIndex = data.AddressIndex,
                    Phone = data.Phone,
                    Province = data.Province,
                    City = data.City,
                    Block = data.Block,
                };
                
            
                db.SellerAddresses.Add(address);
            
                db.SaveChanges();
            
                return Ok("Insert Success");
            }
            var updateselleraddress = db.SellerAddresses.FirstOrDefault(sa =>sa.AddressIndex == data.AddressIndex);
            if (updateselleraddress != null)
            {
                updateselleraddress.Phone = data.Phone;
                updateselleraddress.Province = data.Province;
                updateselleraddress.City = data.City;
                updateselleraddress.Block = data.Block;
                db.SaveChanges();
                return Ok("Update Success");
            }

            return BadRequest("Unable to Insert and Update");

        }

        
        public class GetRequest
        {
            public int AddressIndex;
        }

        [HttpPost]
        [Route("api/GetSellerAddress")]
        public IHttpActionResult GetCustomerAddress(GetRequest data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var address = db.SellerAddresses.Find(data.AddressIndex);
            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }
    }
}
/*
public class GetRequest
        {
            public int AddressIndex;
        }

        [HttpPost]
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
        */