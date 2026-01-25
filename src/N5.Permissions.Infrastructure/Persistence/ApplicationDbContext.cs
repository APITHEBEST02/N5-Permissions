using Microsoft.EntityFrameworkCore;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Infrastructure.Persistence;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Permission> Permissions { get; set; } = null!;
    public DbSet<PermissionType> PermissionTypes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.NombreEmpleado).IsRequired();
            entity.Property(e => e.ApellidoEmpleado).IsRequired();
            entity.Property(e => e.TipoPermiso).IsRequired();
            entity.Property(e => e.FechaPermiso).IsRequired();

            entity.HasOne(e => e.PermissionType)
                .WithMany(pt => pt.Permissions)
                .HasForeignKey(e => e.TipoPermiso);
        });

        modelBuilder.Entity<PermissionType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Descripcion).IsRequired();
        });
    }
}