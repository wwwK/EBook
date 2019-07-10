using System.Threading.Tasks;
using System.Web;
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
    public class ManageCustomersController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [HttpPost]
        [Route("api/ManageSelectCustomer")]
        public IHttpActionResult ManageSelectCustomer()
        {
            Customer[] tableCustomers = db.Customers.ToArray();
            IEnumerable<Customer> selectTableCustomers =
                from customer in tableCustomers
                select customer;
            return Ok(selectTableCustomers.ToArray());

        }
        
        
        [HttpPost]
        [Route("api/ManageInsertCustomer")]
        public IHttpActionResult ManageInsertCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Customers.Add(customer);
            db.SaveChanges();

            return Ok("Insert Success");
        }
        
    }
}