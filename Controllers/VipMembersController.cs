using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using EBook.Models;

namespace EBook.Controllers
{
    public class VipMembersController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/VipMembers
        public IQueryable<VipMember> GetVipMembers()
        {
            return db.VipMembers;
        }

        // GET: api/VipMembers/5
        [ResponseType(typeof(VipMember))]
        public async Task<IHttpActionResult> GetVipMember(int id)
        {
            VipMember vipMember = await db.VipMembers.FindAsync(id);
            if (vipMember == null)
            {
                return NotFound();
            }

            return Ok(vipMember);
        }

        // PUT: api/VipMembers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVipMember(int id, VipMember vipMember)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vipMember.CustomerId)
            {
                return BadRequest();
            }

            db.Entry(vipMember).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VipMemberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/VipMembers
        [ResponseType(typeof(VipMember))]
        public async Task<IHttpActionResult> PostVipMember(VipMember vipMember)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VipMembers.Add(vipMember);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VipMemberExists(vipMember.CustomerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = vipMember.CustomerId }, vipMember);
        }

        // DELETE: api/VipMembers/5
        [ResponseType(typeof(VipMember))]
        public async Task<IHttpActionResult> DeleteVipMember(int id)
        {
            VipMember vipMember = await db.VipMembers.FindAsync(id);
            if (vipMember == null)
            {
                return NotFound();
            }

            db.VipMembers.Remove(vipMember);
            await db.SaveChangesAsync();

            return Ok(vipMember);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VipMemberExists(int id)
        {
            return db.VipMembers.Count(e => e.CustomerId == id) > 0;
        }
    }
}