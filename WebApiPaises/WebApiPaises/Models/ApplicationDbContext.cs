using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPaises.Models
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser> /*Para usar las tablas del sistema de usuarios de asp.net core. voy a cambiar de DbContext a IdentityDbContext*/
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {}
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
    }
}
