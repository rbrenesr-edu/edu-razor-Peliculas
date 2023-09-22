using Peliculas.Shared.Entities;

namespace Peliculas.Client.Repositorios
{
    public interface IRepositorio
    {
        List<Pelicula> ObtenerPeliculas();
    }
}
