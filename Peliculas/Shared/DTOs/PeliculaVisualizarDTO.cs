using Peliculas.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peliculas.Shared.DTOs
{
    public class PeliculaVisualizarDTO
    {
        public Pelicula? Pelicula { get; set; }
        public List<Actor>? Actores { get; set; } = new List<Actor>();
        public List<Genero>? Generos { get; set; } = new List<Genero>();

        public int VotoUsuario { get; set; }
        public double PromedioVotos { get; set; }
    }
}
