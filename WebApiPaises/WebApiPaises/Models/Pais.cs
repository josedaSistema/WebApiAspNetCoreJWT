using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPaises.Models
{
    public class Pais
    {

        public Pais()
        {
            Provincias = new List<Provincia>();
        }
        public int ID { get; set; }
        [StringLength(30)]
        public string Nombre { get; set; }
        public ICollection<Provincia> Provincias { get; set; }
    }
}
