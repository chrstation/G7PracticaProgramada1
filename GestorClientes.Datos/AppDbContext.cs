using GestorClientes.Entidades;
using Microsoft.EntityFrameworkCore;

namespace GestorClientes.Datos;

/// <summary>
/// Contexto de Entity Framework Core para la base de datos de clientes.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();

    public DbSet<Telefono> Telefonos => Set<Telefono>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entidad =>
        {
            entidad.HasKey(c => c.Id);
            entidad.Property(c => c.Cedula).IsRequired().HasMaxLength(20);
            entidad.HasIndex(c => c.Cedula).IsUnique();
            entidad.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
            entidad.Property(c => c.Apellido).IsRequired().HasMaxLength(100);
            entidad.Property(c => c.Correo).HasMaxLength(150);
            entidad.Property(c => c.Direccion).HasMaxLength(250);
            entidad.Ignore(c => c.NombreCompleto);

            // Al borrar un cliente se borran sus teléfonos (cascada).
            entidad.HasMany(c => c.Telefonos)
                   .WithOne(t => t.Cliente)
                   .HasForeignKey(t => t.ClienteId)
                   .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Telefono>(entidad =>
        {
            entidad.HasKey(t => t.Id);
            entidad.Property(t => t.Numero).IsRequired().HasMaxLength(20);
            entidad.Property(t => t.Tipo).HasMaxLength(20);
        });
    }
}
