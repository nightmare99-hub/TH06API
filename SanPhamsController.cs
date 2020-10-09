using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BackendOfTH6.Models;

namespace BackendOfTH6.Controllers
{
    public class SanPhamsController : ApiController
    {
        private Th6Entities db = new Th6Entities();

        // GET: api/SanPhams
        [Route("SanPhams/all")]
        public IQueryable<SanPham> GetSanPham()
        {
            return db.SanPham;
        }

        // GET: api/SanPhams/5
        [ResponseType(typeof(SanPham))]
        public IHttpActionResult GetSanPham(string id)
        {
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return Ok(sanPham);
        }
        [Route("SanPhams/by-ma-danh-muc/{id}")]
        [ResponseType(typeof(SanPham))]
        public IQueryable<SanPham> GetSanPhamByMaDanhMuc(int id)
        {
            var sanPham = db.SanPham.Where(s=>s.MaDanhMuc==id);
            if (sanPham == null)
            {
                return null;
            }

            return sanPham;
        }
        [Route("SanPhams/by-name/{ten}")]
        public IQueryable<SanPham> GetSanPhamTen(string ten)
        {
           var sanPham = db.SanPham.Where(s=>s.Ten.Contains(ten));
            if (sanPham != null)
                return sanPham;
            else return null;
        }
        [Route("SanPhams/by-prices/{min}/{max}")]
        public IQueryable<SanPham> GetGiaMinMax(int min, int max)
        {
            IQueryable<SanPham> sanPham;
            if (min > max) {
                 sanPham = db.SanPham.Where(s=>s.DonGia<=min&&s.DonGia>max);
            }
            else {
                 sanPham = db.SanPham.Where(s => s.DonGia >= min && s.DonGia <= max);
            }
            if (sanPham != null)
                return sanPham;
            else return null;
        }
        [Route("SanPhams/by-price/{gia}")]
        public IQueryable<SanPham> GetGia(int gia)
        {
            var sanPham = db.SanPham.Where(s => Math.Abs(s.DonGia.Value-gia) <= 1000000);
            if (sanPham != null)
                return sanPham;
            else return null;
        }
        // PUT: api/SanPhams/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSanPham(string id, SanPham sanPham)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sanPham.Ma)
            {
                return BadRequest();
            }

            db.Entry(sanPham).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SanPhamExists(id))
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

        // POST: api/SanPhams
        [ResponseType(typeof(SanPham))]
        public IHttpActionResult PostSanPham(SanPham sanPham)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SanPham.Add(sanPham);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SanPhamExists(sanPham.Ma))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = sanPham.Ma }, sanPham);
        }

        // DELETE: api/SanPhams/5
        [Route("SanPhams/xoa")]
        [ResponseType(typeof(SanPham))]
        public IHttpActionResult DeleteSanPham(string id)
        {
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return NotFound();
            }

            db.SanPham.Remove(sanPham);
            db.SaveChanges();

            return Ok(sanPham);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SanPhamExists(string id)
        {
            return db.SanPham.Count(e => e.Ma == id) > 0;
        }
    }
}