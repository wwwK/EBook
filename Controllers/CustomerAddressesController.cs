﻿using System;
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
    public class CustomerAddressesController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/CustomerAddresses
        public IQueryable<CustomerAddress> GetCustomerAddresses()
        {
            return db.CustomerAddresses;
        }

        // GET: api/CustomerAddresses/5
        [ResponseType(typeof(CustomerAddress))]
        public async Task<IHttpActionResult> GetCustomerAddress(int id)
        {
            CustomerAddress customerAddress = await db.CustomerAddresses.FindAsync(id);
            if (customerAddress == null)
            {
                return NotFound();
            }

            return Ok(customerAddress);
        }

        // PUT: api/CustomerAddresses/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCustomerAddress(int id, CustomerAddress customerAddress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerAddress.AddressIndex)
            {
                return BadRequest();
            }

            db.Entry(customerAddress).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerAddressExists(id))
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

        // POST: api/CustomerAddresses
        [ResponseType(typeof(CustomerAddress))]
        public async Task<IHttpActionResult> PostCustomerAddress(CustomerAddress customerAddress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CustomerAddresses.Add(customerAddress);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = customerAddress.AddressIndex }, customerAddress);
        }

        // DELETE: api/CustomerAddresses/5
        [ResponseType(typeof(CustomerAddress))]
        public async Task<IHttpActionResult> DeleteCustomerAddress(int id)
        {
            CustomerAddress customerAddress = await db.CustomerAddresses.FindAsync(id);
            if (customerAddress == null)
            {
                return NotFound();
            }

            db.CustomerAddresses.Remove(customerAddress);
            await db.SaveChangesAsync();

            return Ok(customerAddress);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerAddressExists(int id)
        {
            return db.CustomerAddresses.Count(e => e.AddressIndex == id) > 0;
        }
    }
}