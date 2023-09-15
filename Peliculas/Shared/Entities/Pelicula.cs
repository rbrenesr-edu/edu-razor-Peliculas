using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peliculas.Shared.Entities
{
    public class Pelicula
    {
        //public string? Titulo { get; set; }
        //public required string Titulo { get; set; }
        public string Titulo { get; set; } = null!;
        public string Titulo2 { get; set; } = null!;

        public DateTime FechaLanzamiento { get; set; }
    }
}

