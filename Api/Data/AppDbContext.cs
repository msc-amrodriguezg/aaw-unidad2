using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Producto> Productos => Set<Producto>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("Productos");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Descripcion).IsRequired().HasMaxLength(500);
                entity.Property(p => p.Precio).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Producto>().HasData(
                new Producto { Id = 1, Nombre = "Laptop", Descripcion = "Laptop gamer", Precio = 1500m },
                new Producto { Id = 2, Nombre = "Mouse", Descripcion = "Mouse inalámbrico", Precio = 25.50m }
            );
        }
    }
}
