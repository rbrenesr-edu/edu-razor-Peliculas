using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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

            if(pelicula.PeliculasActor is not null)
            {
                for (int i = 0; i < pelicula.PeliculasActor.Count; i++)
                {
                    pelicula.PeliculasActor[i].Orden = i + 1;
                }
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PeliculaVisualizarDTO>> Get(int id) {

            var pelicula = await context.Peliculas
                .Where(pelicula => pelicula.Id == id)
                .Include(pelicula => pelicula.GenerosPelicula)
                    .ThenInclude(gp => gp.Genero)
                 .Include(pelicula => pelicula.PeliculasActor.OrderBy(pa => pa.Orden))
                    .ThenInclude(pl => pl.Actor)
                .FirstOrDefaultAsync();

            if (pelicula is null)
            {
                return NotFound();
            }

            // TODO: Sistema de votación
            var promedioVoto = 4;
            var votoUsuario = 5;

            var modelo = new PeliculaVisualizarDTO();
            modelo.Pelicula = pelicula;
            modelo.Generos = pelicula.GenerosPelicula.Select(x => x.Genero!).ToList();
            modelo.Actores = pelicula.PeliculasActor.Select( x => new Actor 
            { 
                Nombre = x.Actor!.Nombre,
                Foto = x.Actor.Foto,
                Personaje = x.Personaje,
                Id = x.Actor.Id
            }).ToList();

            modelo.VotoUsuario = votoUsuario;
            modelo.PromedioVotos = promedioVoto;

            return modelo;
        }

        [HttpGet("actualizar/{id}")]
        public async Task<ActionResult<PeliculaActualizacionDTO>> PutGet(int id) {
            var peliculaActionresult = await Get(id);
            if (peliculaActionresult.Result is NotFoundResult) { return NotFound(); }

            var peliculaBase = peliculaActionresult.Value;
            var generosSeleccionadosIds = peliculaBase!.Generos.Select(x=>x.ID).ToList();
            var generosNoSeleccionados = await context.Generos
                .Where(x => !generosSeleccionadosIds.Contains(x.ID))
                .ToListAsync();

            var modelo = new PeliculaActualizacionDTO();
            modelo.Pelicula = peliculaBase.Pelicula;
            modelo.GenerosSeleccionados = peliculaBase.Generos;
            modelo.GenerosNoSeleccionados = generosNoSeleccionados;
            modelo.Actores = peliculaBase.Actores;

            return modelo;

        }

        [HttpPut]
        public async Task<ActionResult> Put(Actor actor)
        {
            context.Update(actor);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
