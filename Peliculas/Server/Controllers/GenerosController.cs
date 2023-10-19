using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Peliculas.Shared.Entities;

namespace Peliculas.Server.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController: ControllerBase
    {
        private readonly ApplicationDbContext context;

        public GenerosController( ApplicationDbContext context ) { 
            this.context = context;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Genero genero) {            
            context.Add(genero);
            await context.SaveChangesAsync();
            return genero.ID;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genero>>> Get()
        {
            return await context.Generos.ToListAsync();
        }

    }
}
