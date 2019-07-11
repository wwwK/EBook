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
    public class ManageSellerAddressesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [HttpPost]
        [Route("api/ManageSelectSellerAddress")]
        public IHttpActionResult ManageSelectSellerAddress()
        {
            SellerAddress [] tableSellerAddresses = db.SellerAddresses.ToArray();
            IEnumerable<SellerAddress>selectTableSellerAddresses =
                from sellerAddress in tableSellerAddresses
                select sellerAddress;
            return Ok(selectTableSellerAddresses.ToArray());

        }
        
        
        [HttpPost]
        [Route("api/ManageInsertSellerAddress")]
        public IHttpActionResult ManageInsertSellerAddress(SellerAddress sellerAddress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.SellerAddresses.Add(sellerAddress);
            db.SaveChanges();

            return Ok("Insert Success");
        }
        
        [HttpPost]
        [Route("api/ManageUpdateSellerAddress")]
        public IHttpActionResult ManageUpdateSellerAddress(SellerAddress sellerAddress)
        {
            SellerAddress updatedSellerAddress = db.SellerAddresses.FirstOrDefault(sa => sa.AddressIndex == sellerAddress.AddressIndex);
            if (updatedSellerAddress != null)
            {
                updatedSellerAddress = sellerAddress;
                db.SaveChanges();
                return Ok("更新成功！");
            }

            return BadRequest("请重新操作！");
        }
        
    }
}