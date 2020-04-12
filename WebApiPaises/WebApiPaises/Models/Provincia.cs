using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPaises.Models
{
    public class Provincia
    {
        
        public int ID { get; set; }
        public string Nombre { get; set; }
        [ForeignKey("Pais")]
        public int PaisID { get; set; }
        //JsonIGnore me permite ignorar esa propiedad para  cuando se consulte el modelo.
        [JsonIgnore]
        public Pais Pais { get; set; }
    }
}
