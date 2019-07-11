using System.Web.Http;
using EBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using BCrypt.Net;
using System.Web.SessionState;
using NETCore.Encrypt;
using EBook.Service;


namespace EBook.Controllers
{
    public class ManageCustomerAddressesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [HttpPost]
        [Route("api/ManageSelectCustomerAddress")]
        public IHttpActionResult ManageSelectCustomerAddress()
        {
            CustomerAddress [] tableCustomerAddresses = db.CustomerAddresses.ToArray();
            IEnumerable<CustomerAddress>selectTableCustomerAddresses =
                from customerAddress in tableCustomerAddresses
                select customerAddress;
            return Ok(selectTableCustomerAddresses.ToArray());

        }
        
        
        [HttpPost]
        [Route("api/ManageInsertCustomerAddress")]
        public IHttpActionResult ManageInsertCustomerAddress(CustomerAddress customerAddress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.CustomerAddresses.Add(customerAddress);
            db.SaveChanges();

            return Ok("Insert Success");
        }
        
        [HttpPost]
        [Route("api/ManageUpdateCustomerAddress")]
        public IHttpActionResult ManageUpdateCustomerAddress(CustomerAddress customerAddress)
        {
            CustomerAddress updatedCustomerAddress = db.CustomerAddresses.FirstOrDefault(ca => ca.AddressIndex == customerAddress.AddressIndex);
            if (updatedCustomerAddress != null)
            {
                updatedCustomerAddress.IsValid = 0;
                db.SaveChanges();
                return Ok("更新成功！");
            }

            return BadRequest("请重新操作！");
        }
        
    }


}