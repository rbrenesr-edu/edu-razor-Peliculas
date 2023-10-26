using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Peliculas.Server.Helpers;
using Peliculas.Shared.DTOs;
using Peliculas.Shared.Entities;

namespace Peliculas.Server.Controllers
{
    [Route("api/actores")]
    [ApiController]
    public class ActoresController : ControllerBase        
    {

        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IMapper mapper;
        private readonly string contenedor = "personas";

        public ActoresController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos, IMapper mapper )
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
            this.mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult<int>> Post(Actor actor)
        {
            if (!string.IsNullOrWhiteSpace(actor.Foto))
            {
                var fotoActor = Convert.FromBase64String(actor.Foto);
                actor.Foto = await almacenadorArchivos.GuardarArchivo(fotoActor, ".jpg", contenedor);
            }

            context.Add(actor);
            await context.SaveChangesAsync();
            return actor.Id;
        }

        [HttpGet] //[FromQuery]PaginacionDTO paginacion    =>  url?pagina=1&cantidadRegistros=10
        public async Task<ActionResult<IEnumerable<Actor>>> Get([FromQuery]PaginacionDTO paginacion)
        {
            //return await context.Actores.OrderBy(x=>x.Nombre).ToListAsync();
            var queryable = context.Actores.AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacion.CantidadRegistros);

            return await queryable.OrderBy(x=>x.Nombre).Paginar(paginacion).ToListAsync();

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Actor>> Get(int id)
        {
            //return await context.Generos.Where(x => x.ID == id).FirstOrDefaultAsync();
            var actor = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (actor is null)
            {
                return NotFound();
            }

            return actor;
        }

        [HttpGet("buscar/{textoBuscar}")]
        public async Task<ActionResult<IEnumerable<Actor>>> Get(string textoBuscar)
        {
            if (string.IsNullOrWhiteSpace(textoBuscar))
            {
                return new List<Actor>();
            }

            return await context.Actores
                .Where(actor => actor.Nombre!.ToLower().Contains(textoBuscar.ToLower()))
                .OrderByDescending(actor => actor.Nombre)  // Agrega la cláusula OrderBy aquí
                .Take(5)
                .ToListAsync();

        }

        [HttpPut]
        public async Task<ActionResult> Put(Actor actor)
        {
            var actorBD = await context.Actores.FirstOrDefaultAsync(x => x.Id == actor.Id);

            if (actorBD is null) {
                return NotFound();
            }

            actorBD = mapper.Map(actor,actorBD);

            if (!string.IsNullOrWhiteSpace(actor.Foto)) {
                var fotoActor = Convert.FromBase64String(actor.Foto);
                actorBD.Foto = await almacenadorArchivos.EditarArchivo(
                    fotoActor, ".jpg", contenedor, actorBD.Foto!);
            }
            
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var actor = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (actor is null) {
                return NotFound();
            }

            context.Remove(actor);
            await context.SaveChangesAsync();
            await almacenadorArchivos.EliminarArchivo(actor.Foto!, contenedor);
        
            return NoContent();
        }
    }
}
