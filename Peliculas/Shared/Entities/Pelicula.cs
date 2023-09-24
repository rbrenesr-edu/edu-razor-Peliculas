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
        public DateTime FechaLanzamiento { get; set; }
        public string? Poster { get; set; }
        public string TituloCorto
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Titulo))
                {
                    return null;
                }
                if (Titulo.Length > 60)
                {
                    return Titulo.Substring(0, 60) + "...";
                }
                else
                {
                    return Titulo;
                }
            }
        }
    }
}

