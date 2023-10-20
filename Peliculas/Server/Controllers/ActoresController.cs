using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Peliculas.Server.Helpers;
using Peliculas.Shared.Entities;

namespace Peliculas.Server.Controllers
{
    [Route("api/actores")]
    [ApiController]
    public class ActoresController : ControllerBase        
    {

        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "personas";

        public ActoresController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> Get()
        {
            return await context.Actores.ToListAsync();
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
    }
}
