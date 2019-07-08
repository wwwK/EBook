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



        public class SearchDate
        {
            public string searchinfo;
        }


         
 
        [HttpPost]
        [Route("api/CouponSearch/")]
        public IHttpActionResult CouponSearchWithShopName(SearchDate data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CouponInfo[] coupons = CouponSearch.CouponSearchWithShopName(data.searchinfo);
            if (coupons.Length == 0)
            {
                return BadRequest("No Coupons Found");
            }
            return Ok(coupons);
        }
         
        /*[HttpGet]
        [Route("api/CouponSearch/{SearchString}")]
        public async Task<IHttpActionResult> mohuchhhh(SearchDate data)
        {
 
             
            CouponSearch a = new CouponSearch();
            
            return Ok(a.CouponSearchWithShopName(data.searchinfo));
        }*/
 
         
    }
 
}