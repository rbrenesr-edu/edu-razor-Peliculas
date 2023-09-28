using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peliculas.Shared.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        
        
        [Required]        
        public string? Nombre { get; set; }
        public string? Biografia { get; set; }
        public string? Foto { get; set; }
        public DateTime? FechaNacimiento { get; set; }

    }
}
