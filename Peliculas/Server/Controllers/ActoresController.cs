using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Peliculas.Shared.Entities;

namespace Peliculas.Server.Controllers
{
    [Route("api/actores")]
    [ApiController]
    public class ActoresController : ControllerBase        
    {

        private readonly ApplicationDbContext context;
        public ActoresController(ApplicationDbContext context)
        {
            this.context = context;
        }


        [HttpPost]
        public async Task<ActionResult<int>> Post(Actor actor)
        {
            context.Add(actor);
            await context.SaveChangesAsync();
            return actor.Id;
        }
    }
}
