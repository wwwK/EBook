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
    public class ManageSellersController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [HttpPost]
        [Route("api/ManageSelectSeller")]
        public IHttpActionResult ManageSelectSeller()
        {
            Seller[] tableSellers = db.Sellers.ToArray();
            IEnumerable<Seller> selectTableSellers =
                from seller in tableSellers
                select seller;
            return Ok(selectTableSellers.ToArray());

        }
        
        
        [HttpPost]
        [Route("api/ManageInsertSeller")]
        public IHttpActionResult ManageInsertSeller(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Sellers.Add(seller);
            db.SaveChanges();

            return Ok("Insert Success");
        }
        
        [HttpPost]
        [Route("api/ManageUpdateSeller")]
        public IHttpActionResult ManageUpdateSeller(Seller seller)
        {
            Seller updatedSeller = db.Sellers.FirstOrDefault(s => s.SellerId == seller.SellerId);
            if (updatedSeller != null)
            {
                updatedSeller.IsValid = 0;
                db.SaveChanges();
                return Ok("更新成功！");
            }

            return BadRequest("请重新操作！");
        }
        
    }
}