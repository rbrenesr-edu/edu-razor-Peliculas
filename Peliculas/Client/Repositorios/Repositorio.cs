using Peliculas.Shared.Entities;

namespace Peliculas.Client.Repositorios
{
    public class Repositorio : IRepositorio
    {
        public List<Pelicula> ObtenerPeliculas()
        {
            return new List<Pelicula>()
            {
                new Pelicula{ Titulo = "Wakanda Forever", FechaLanzamiento = new DateTime(2016,11,25) },
                new Pelicula{ Titulo = "Moana", FechaLanzamiento = new DateTime(2020,10,25) },
                new Pelicula{ Titulo = "inception", FechaLanzamiento = new DateTime(2023,9,15) }
            };
        }
    }
}
