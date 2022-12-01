using Microsoft.EntityFrameworkCore;
using WebApiCasino.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApiCasino
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ParticipanteRifaCarta>()
                .HasKey(al => new { al.IdParticipante, al.IdRifa, al.IdCarta });
        }

        public DbSet<Carta> Cartas { get; set; }
        public DbSet<Participante> Participantes { get; set; }
        public DbSet<ParticipanteRifaCarta> ParticipanteRifaCarta { get; set; }
        public DbSet<Premios> Premios { get; set; }
        public DbSet<Rifa> Rifas { get; set; }
 
    }
}
