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


namespace EBook.Controllers
{
    public class SellerController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        
        public class RegisterData
        {
            public Seller SellerData;
            public string ValidateCode;
        }

        [HttpPost]
        [Route("api/Seller/")]
        public IHttpActionResult UpdateUser(RegisterData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tmpResult = Service.EmailSend.CheckVerifyCode(data.SellerData.SellerEmail, data.ValidateCode);
            if (tmpResult != 0)
            {
                switch (tmpResult)
                {
                    case -1:
                        return BadRequest("Validate code not sent.");
                    case -2:
                        return BadRequest("Wrong validate code.");
                    case -3:
                        return BadRequest("Validate code expired.");
                }
            }
            

//            Seller seller = new Seller
//            {
//                Password = data.SellerData.Password,
//                ShopName = data.SellerData.ShopName,
//                CreditLevel = data.SellerData.CreditLevel,
//                ShopDescription = data.SellerData.ShopDescription,
//                SellerPhone = data.SellerData.SellerPhone,
//
//            };
//          



            Seller seller =new Seller();
            
            seller = data.SellerData;

            var inserted = db.Sellers.Add(seller);

            db.SaveChanges();

            var cookie = new HttpCookie("sessionId")
            {
                Value = Service.CustomerSession.SetSessionId(inserted.SellerId).ToString(),
                Expires = DateTime.Now.AddHours(1)
            };

            HttpContext.Current.Response.Cookies.Add(cookie);


            return Ok();
        }
        [HttpPost]
        [Route("api/Seller/1")]
        public IHttpActionResult InsertSeller(Seller data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            Seller seller = new Seller
            {
                SellerId = data.SellerId,
                Password = data.Password,
                ShopName = data.ShopName,
                CreditLevel = data.CreditLevel,
                ShopDescription = data.ShopDescription,
                SellerPhone = data.SellerPhone,

            };

            db.Sellers.Add(seller);
            

            db.SaveChanges();
            

            return Ok();
        }

        public class GetRequest
        {
            public int SellerId;
        }

        [HttpGet]
        [Route("api/Seller/1")]
        public IHttpActionResult GetMerchandise(GetRequest data)
        {
            var seller = db.Sellers.Find(data.SellerId);
            if (seller == null)
            {
                return NotFound();
            }

            return Ok(seller);
        }
    }
}