using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPaises.Models;

namespace WebApiPaises.Controllers
{
    [Produces("application/json")]
    [Route("api/Pais/{PaisID}/Provincias")]
    
    public class ProvinciasController : Controller
    {
        private readonly ApplicationDbContext  context;
        public ProvinciasController(ApplicationDbContext a)
        {
            context = a;
        }
        [HttpGet]
        public IEnumerable<Provincia> GetAll(int PaisID)
        {
            return context.Provincias.Where(x=>x.PaisID== PaisID).ToList();
        }
        [HttpGet("{id}", Name = "ProvinciaCreado")]
        public IActionResult GetbyID(int id)
        {
            var pais = context.Paises.FirstOrDefault(x => x.ID == id);
            if (pais == null)
            {
                return NotFound();
            };
            return Ok(pais);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Provincia provincia,int PaisID)
        {
            provincia.PaisID = PaisID;
            if (ModelState.IsValid)
            {
                context.Provincias.Add(provincia);
                context.SaveChanges();
                return new CreatedAtRouteResult("ProvinciaCreado", new { id = provincia.ID }, provincia);
            }

            return BadRequest(ModelState);

        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Provincia provincia, int id, int PaisID)
        {
            provincia.PaisID = PaisID;
            if (provincia.ID != id)
            {
                return BadRequest(provincia);
            }
            context.Entry(provincia).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id ,int PaisID)
        {
            var pais = context.Provincias.FirstOrDefault(x => x.ID == id && x.PaisID==id);
            if (pais == null)
            {
                return NotFound();
            }
            context.Provincias.Remove(pais);
            context.SaveChanges();
            return Ok(pais);
        }

    }
}