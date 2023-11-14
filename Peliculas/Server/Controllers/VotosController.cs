using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Peliculas.Shared.DTOs;
using Peliculas.Shared.Entities;

namespace Peliculas.Server.Controllers
{
    [ApiController]
    [Route("api/votos")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VotosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMapper mapper;

        public VotosController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IMapper mapper) {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Votar(VotoPeliculaDTO votoPeliculaDTO) {
            var usuario = await userManager.FindByEmailAsync(HttpContext.User.Identity!.Name!);

            if (usuario is null)
            {
                return BadRequest("Usuario no encontrado!");
            }

            var usuarioID = usuario.Id;

            var votoActual = await context.VotsPeliculas
                .FirstOrDefaultAsync(x => x.PeliculaId == votoPeliculaDTO.PeliculaId && x.UsuarioId == usuarioID);

            if (votoActual is null)
            {
                var vp = mapper.Map<VotoPelicula>(votoPeliculaDTO);
                vp.UsuarioId = usuarioID;
                vp.FechaVoto = DateTime.Now;
                context.Add(vp);
            }
            else
            {
                votoActual.FechaVoto = DateTime.Now;
                votoActual.Voto = votoPeliculaDTO.Voto;
            }

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
