using Microsoft.EntityFrameworkCore;

namespace Peliculas.Server
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
