using Microsoft.EntityFrameworkCore;
using WebApiCasino.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApiCasino
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Carta> Cartas { get; set; }
    }
}
