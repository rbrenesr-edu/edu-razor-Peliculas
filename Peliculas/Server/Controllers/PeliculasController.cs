using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Peliculas.Server.Helpers;
using Peliculas.Shared.DTOs;
using Peliculas.Shared.Entities;

namespace Peliculas.Server.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "peliculas";

        public PeliculasController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Pelicula pelicula)
        {
            if (!string.IsNullOrWhiteSpace(pelicula.Poster))
            {
                var peliculaPoster = Convert.FromBase64String(pelicula.Poster);
                pelicula.Poster = await almacenadorArchivos.GuardarArchivo(peliculaPoster, ".jpg", contenedor);
            }

            context.Add(pelicula);
            await context.SaveChangesAsync();
            return pelicula.Id;
        }

        [HttpGet]
        public async Task<ActionResult<HomePageDTO>> Get()
        {

            int limite = 6;

            var peliculasCartelera = await context.Peliculas
                .Where(pelicula => pelicula.EnCartelera)
                .OrderBy(pelicula => pelicula.Lanzamiento)
                .Take(limite)
                .ToListAsync();

            var fechaActual = DateTime.Today;

            var proximosExtrenos = await context.Peliculas
              .Where(pelicula => pelicula.Lanzamiento > fechaActual)
              .OrderBy(pelicula => pelicula.Lanzamiento)
              .Take(limite)
              .ToListAsync();

            var result = new HomePageDTO
            {
                PeliculasEnCartelera = peliculasCartelera,
                ProximosExtrenos = proximosExtrenos
            };

            return result;
        }
    }
}
