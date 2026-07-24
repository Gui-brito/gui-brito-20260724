using GestaoColaboradores.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoColaboradores.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Colaborador> Colaboradores { get; set; }
    public DbSet<Unidade> Unidades { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("usuarios");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Codigo).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Codigo).IsUnique();
            entity.Property(e => e.Login).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Login).IsUnique();
            entity.Property(e => e.SenhaHash).IsRequired();
            entity.Property(e => e.Ativo).HasDefaultValue(true);
        });

        // Unidade
        modelBuilder.Entity<Unidade>(entity =>
        {
            entity.ToTable("unidades");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Codigo).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Codigo).IsUnique();
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Ativa).HasDefaultValue(true);
        });

        // Colaborador
        modelBuilder.Entity<Colaborador>(entity =>
        {
            entity.ToTable("colaboradores");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Codigo).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Codigo).IsUnique();
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);

            entity.HasOne(e => e.Unidade)
                  .WithMany(u => u.Colaboradores)
                  .HasForeignKey(e => e.UnidadeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Usuario)
                  .WithOne(u => u.Colaborador)
                  .HasForeignKey<Colaborador>(e => e.UsuarioId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.AtualizadoEm = DateTime.UtcNow;
            }
        }
    }
}
