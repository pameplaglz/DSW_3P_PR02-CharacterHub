using CharacterHub.Models;
using Microsoft.EntityFrameworkCore;

namespace CharacterHub.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Personaje> Personajes => Set<Personaje>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Personaje>(entity =>
        {
            entity.ToTable("Personajes");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
            entity.Property(p => p.Videojuego).IsRequired().HasMaxLength(150);
            entity.Property(p => p.Clase).IsRequired().HasMaxLength(100);
            entity.Property(p => p.ImagenUrl).IsRequired().HasMaxLength(500);
        });
    }
}
