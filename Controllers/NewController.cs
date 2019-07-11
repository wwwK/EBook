using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EBook.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using BCrypt.Net;
using System.Web.SessionState;
using EBook.Service;


namespace EBook.Controllers
{
    public class NewController : ApiController
    {
        public class GetRequest
        {
            public int MerchandiseId;
            public int TransactId;
            public string Comment;
            public string LogisticTrackNum;
            public int Status;
        }
        
        private OracleDbContext db = new OracleDbContext();

        
        [HttpPost]
        [Route("api/SellerUpdateTransact")]
        public IHttpActionResult SellerUpdateTransact(GetRequest data)
        {
            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return BadRequest("Not Logged in");
            }

            var currentSellerId = Service.SellerSession.GetSellerIdFromSession(int.Parse(session.Value));
            if (currentSellerId < 0)
            {
                return BadRequest("Not Logged in");
            }

            var currentTransact = db.Transacts.FirstOrDefault(t => t.TransactId == data.TransactId);
            if (currentTransact == null)
            {
                return BadRequest("No Transact Found");
            }
            int currentMerchandiseId = currentTransact.MerchandiseId;
            if (db.Merchandises.FirstOrDefault(m => m.MerchandiseId == currentMerchandiseId).SellerId !=
                currentSellerId)
            {
                return BadRequest("No Authority");
            }

            currentTransact.LogisticTrackNum = data.LogisticTrackNum;
            currentTransact.Status = data.Status;
            db.SaveChanges();
            return Ok();

        }
        
        
        [HttpPost]
        [Route("api/CustomerDeleteAddress/")]
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


            var customerAddress = db.CustomerAddresses.FirstOrDefault(ca =>ca.AddressIndex == data.AddressIndex);
            if (customerAddress.CustomerId != customerId)
            {
                return BadRequest("No Authority");
            }
            if (customerAddress != null)
            {
                customerAddress.IsValid = 0;
                db.SaveChanges();
                return Ok("Delete Success");
            }

            return BadRequest("Unable to Delete");

        }

        [HttpPost]
        [Route("api/CustomerGetAllAddresses")]
        public IHttpActionResult CustomerGetAllAddresses()
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

            CustomerAddress[] customerAddresses = AddressService.CustomerGetAllAddresses(customerId);
            if (customerAddresses.Length == 0)
            {
                return BadRequest("No Address");
            }

            return Ok(customerAddresses);
        }
        
        [HttpPost]
        [Route("api/SellerGetAllAddresses")]
        public IHttpActionResult SellerGetAllAddresses()
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

            SellerAddress[] sellerAddresses = AddressService.SellerGetAllAddresses(sellerId);
            if (sellerAddresses.Length == 0)
            {
                return BadRequest("No Address");
            }

            return Ok(sellerAddresses);
        }

    }
}