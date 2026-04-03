using Microsoft.EntityFrameworkCore;
using RegistroMiembros.Models;

namespace RegistroMiembros.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Miembro> Miembros { get; set; }
    }
}