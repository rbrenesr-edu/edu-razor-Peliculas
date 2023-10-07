using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using Peliculas.Shared.Entities;
=======
>>>>>>> 2707e72b680dd5335ce8efbb436de9d0f079accf

namespace Peliculas.Server
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
<<<<<<< HEAD

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GeneroPelicula>().HasKey(x => new {x.GeneroId, x.PeliculaId });
            modelBuilder.Entity<PeliculaActor>().HasKey(x => new {x.ActorId, x.PeliculaId });
        }

        public DbSet<Genero> Generos => Set<Genero>();
        public DbSet<Actor> Actores => Set<Actor>();
        public DbSet<Pelicula> Peliculas => Set<Pelicula>();
        public DbSet<GeneroPelicula> GenerosPeliculas => Set<GeneroPelicula>();
        public DbSet<PeliculaActor> PeliculasActor => Set<PeliculaActor>();
=======
>>>>>>> 2707e72b680dd5335ce8efbb436de9d0f079accf
    }
}
