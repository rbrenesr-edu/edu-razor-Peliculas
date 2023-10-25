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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Genero>> Get(int id) {
            //return await context.Generos.Where(x => x.ID == id).FirstOrDefaultAsync();
            var genero =  await context.Generos.FirstOrDefaultAsync(x => x.ID == id);

            if (genero is null)
            {
                return NotFound();
            }

            return genero;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Genero genero) { 
            context.Update(genero);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id) {
            var rowsAfected = await context.Generos
                .Where(x => x.ID == id)
                .ExecuteDeleteAsync();


            if (rowsAfected == 0)
            {
                return NotFound();
            }

            return NoContent();


        }


    }
}
