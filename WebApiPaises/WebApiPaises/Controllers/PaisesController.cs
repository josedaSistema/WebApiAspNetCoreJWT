using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiPaises.Models;

namespace WebApiPaises.Controllers
{
    [Produces("application/json")]
    [Route("api/Paises")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaisesController : Controller
    {
        private readonly ApplicationDbContext ApplicationDbContext;
        public PaisesController(ApplicationDbContext applicationDbContext)
        {
            this.ApplicationDbContext = applicationDbContext;
        }
        [HttpGet]
        //[Route("Get")]
        public IActionResult Get()
        {
            var claims = User.Claims.ToList();

            var esAdmin = claims.Any(x => x.Type == "Admin" && x.Value == "Y");

            if (esAdmin)
            {
                return Ok(ApplicationDbContext.Paises.ToList());
            }
            else
            {
                var pais = claims.FirstOrDefault(x => x.Type == "Pais");
                if (pais ==null)
                {
                    return Unauthorized();
                }
                return Ok(ApplicationDbContext.Paises.Where(X => X.Nombre == pais.Value));
            }

            
        }

        [HttpGet("{id}", Name = "PaisCreado")]
        public IActionResult GetbyID(int id)
        {
            var pais = ApplicationDbContext.Paises.Include(x => x.Provincias).FirstOrDefault(x => x.ID == id);
            if (pais == null)
            {
                return NotFound();
            };
            return Ok(pais);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Pais pais)
        {
            if (ModelState.IsValid)
            {
                ApplicationDbContext.Paises.Add(pais);
                ApplicationDbContext.SaveChanges();
                return new CreatedAtRouteResult("PaisCreado", new { id = pais.ID }, pais);
            }

            return BadRequest(ModelState);

        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Pais pais, int id)
        {
            if (pais.ID != id)
            {
                return BadRequest(pais);
            }
            ApplicationDbContext.Entry(pais).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            ApplicationDbContext.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var pais = ApplicationDbContext.Paises.FirstOrDefault(x => x.ID == id);
            if (pais ==null)
            {
                return NotFound();
            }
            ApplicationDbContext.Paises.Remove(pais);
            ApplicationDbContext.SaveChanges();
            return Ok(pais);
        }
    }
   
}