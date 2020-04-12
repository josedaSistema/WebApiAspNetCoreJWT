using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApiPaises.Models;

namespace WebApiPaises
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>(option => 
            option.UseSqlServer(Configuration.GetConnectionString("sqlConnectionString") /*Ojo es ConnectionStrings en el appsetting.json*/));
            
            /*USando  el sistema de Usuario por defecto que nos ofrece asp.net mvc, Nos permite instanciar asp.net core Identity*/
            services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();
            /*Debemos agregar el servicio de Authenticacion de Bearer*/
            /*Las opciones permiten que se utilice el token que viene por desde la peticion para validar sus diferentes parametros */
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "yourdomain.com",
                ValidAudience = "yourdomain.com",
                IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Configuration["Llave_super_secreta"])),
                ClockSkew = TimeSpan.Zero
            });

            services.AddMvc().AddJsonOptions(configureJson);
        }

        private void configureJson(MvcJsonOptions obj)
        {
            //Configurando las Referencias Ciclicas, Sumamente importante
            obj.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            /*Debemos Indicar que use la Authenticación*/
            app.UseAuthentication();

            if (!dbContext.Paises.Any())
            {
                dbContext.Paises.AddRange(new List<Pais>
                {
                    new Pais (){ Nombre="Republica Dominicana", Provincias=new List<Provincia>{
                        new Provincia(){Nombre="Santo Domingo"},
                        new Provincia(){Nombre="Monte Plata"},
                        new Provincia(){Nombre="Samaná"},
                    } },
                    new Pais(){Nombre="Estados Unidos",Provincias=new List<Provincia>{
                        new Provincia(){Nombre="New York"},
                         new Provincia(){Nombre="Washintong"},
                    }},
                     new Pais(){Nombre="Haití"},
                });
                dbContext.SaveChanges();
            }

        }
    }
}
