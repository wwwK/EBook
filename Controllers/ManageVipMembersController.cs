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
    public class ManageVipMembersController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [HttpPost]
        [Route("api/ManageSelectVipMember")]
        public IHttpActionResult ManageSelectVipMember()
        {
            VipMember [] tableVipMembers = db.VipMembers.ToArray();
            IEnumerable<VipMember>selectTableVipMembers =
                from vipMember in tableVipMembers
                select vipMember;
            return Ok(selectTableVipMembers.ToArray());

        }
        
        
        [HttpPost]
        [Route("api/ManageInsertVipMember")]
        public IHttpActionResult ManageInsertVipMember(VipMember vipMember)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.VipMembers.Add(vipMember);
            db.SaveChanges();

            return Ok("Insert Success");
        }
        
        [HttpPost]
        [Route("api/ManageUpdateVipMember")]
        public IHttpActionResult ManageUpdateVipMember(VipMember vipMember)
        {
            VipMember updatedVipMember = db.VipMembers.FirstOrDefault(v => v.CustomerId == vipMember.CustomerId && v.SellerId == vipMember.SellerId);
            if (updatedVipMember != null)
            {
                updatedVipMember = vipMember;
                db.SaveChanges();
                return Ok("更新成功！");
            }

            return BadRequest("请重新操作！");
        }
        
    }



}