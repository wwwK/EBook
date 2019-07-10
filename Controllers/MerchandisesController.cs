using System.Web;
using System.Web.Http;
using EBook.Models;
using System.Linq;
using EBook.Service;


namespace EBook.Controllers
{
    public class MerchandiseController : ApiController
    {
        private readonly OracleDbContext _db = new OracleDbContext();

        
        //insert update
        [HttpPost]
        [Route("api/Merchandise/")]
        public IHttpActionResult InsertMerchandise(Merchandise data)
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

            int sellerId = SellerSession.GetSellerIdFromSession(int.Parse(session.Value));
            if (sellerId < 0)
            {
                return BadRequest("请先登录！");
            }
            if (_db.Merchandises.Find(data.MerchandiseId) == null)
            {
                Merchandise merchandise = new Merchandise
                {
                    SellerId = sellerId,        
                    ISBN = data.ISBN,
                    Description = data.Description,
                    Price = data.Price,
                    IsValid = 1,
                };


                _db.Merchandises.Add(merchandise);
                _db.SaveChanges();

                return Ok("商品上架成功！");
            }

            var updatedMerchandise = _db.Merchandises.FirstOrDefault(m => m.MerchandiseId == data.MerchandiseId);
            if (updatedMerchandise != null)
            {
                updatedMerchandise.ISBN = data.ISBN;
                updatedMerchandise.Description = data.Description;
                updatedMerchandise.Price = data.Price;
                updatedMerchandise.IsValid = data.IsValid;
                _db.SaveChanges();
                return Ok("更新商品成功！");
            }

            return BadRequest("请重新更新商品！");


        }

        public class GetRequest
        {
            public readonly int MerchandiseId;

            public GetRequest(int merchandiseId)
            {
                MerchandiseId = merchandiseId;
            }
        }


        [HttpPost]
        [Route("api/GetMerchandise")]
        public IHttpActionResult GetMerchandise(GetRequest data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
 

            BookInfo[] books = BookSearch.BookSearchWithMerchandiseId(data.MerchandiseId);
            if (books.Length == 0)
            {
                return BadRequest("No Merchandise Found");
            }
            return Ok(books[0]);
        }
//get ok
/*
 ++       
        [HttpGet]
        [Route("api/GetMerchandise")]
        public IHttpActionResult GetMerchandise(GetRequest data)
        {
            var merchandise = db.Merchandises.Find(data.MerchandiseId);
            if (merchandise == null)
            {
                return NotFound();
            }

            return Ok(merchandise);
        }*/

        public class SellerRequest
        {
            public readonly string SellerShopName;

            public SellerRequest(string sellerShopName)
            {
                this.SellerShopName = sellerShopName;
            }
        }
        
        
        [HttpPost]
        [Route("api/GetMerchandisesOfSeller")]
        public IHttpActionResult GetMerchandisesOfSeller(SellerRequest data)
        {
            var result =
                (from seller in _db.Sellers
                    where seller.ShopName == data.SellerShopName
                    join merchandise in _db.Merchandises on seller.SellerId equals merchandise.SellerId
                 select merchandise).ToArray();

            if (result.Length == 0)
            {
                return NotFound();
            }

            return Ok(result);

        }
        
        
        
        
        
    }
}