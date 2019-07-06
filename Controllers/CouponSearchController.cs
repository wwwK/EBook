using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EBook.Models;
using System;
using EBook.Service;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using BCrypt.Net;
 
 
namespace EBook.Controllers
{
     
    public class CouponSearchController : ApiController
    {



        public class searchDate
        {
            public string searchinfo;
        }


         
 
        [HttpPost]
        [Route("api/CouponSearch/")]
        public async Task<IHttpActionResult> CouponSearch(searchDate data)
        {
 
             
            CouponSearch a = new CouponSearch();
            return Ok(a.CouponSearchWithShopName(data.searchinfo));
        }
         
        /*[HttpGet]
        [Route("api/CouponSearch/{SearchString}")]
        public async Task<IHttpActionResult> mohuchhhh(searchDate data)
        {
 
             
            CouponSearch a = new CouponSearch();
            
            return Ok(a.CouponSearchWithShopName(data.searchinfo));
        }*/
 
         
    }
 
}