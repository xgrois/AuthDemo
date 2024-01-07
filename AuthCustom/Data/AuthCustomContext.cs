using AuthCustom.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthCustom.Data
{
    public class AuthCustomContext : DbContext
    {
        public AuthCustomContext(DbContextOptions<AuthCustomContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; } = default!;
        public DbSet<Rol> Roles { get; set; } = default!;
        public DbSet<Permiso> Permisos { get; set; } = default!;
        public DbSet<UsuarioRol> UsuariosRoles { get; set; } = default!;
        public DbSet<RolPermiso> RolesPermisos { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new RolConfiguration());
            modelBuilder.ApplyConfiguration(new PermisoConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioRolConfiguration());
            modelBuilder.ApplyConfiguration(new RolPermisoConfiguration());
        }
    }

    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasIndex(x => x.UserName).IsUnique();
        }
    }

    public class RolConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.HasIndex(x => x.Nombre).IsUnique();
        }
    }

    public class PermisoConfiguration : IEntityTypeConfiguration<Permiso>
    {
        public void Configure(EntityTypeBuilder<Permiso> builder)
        {
            builder.HasIndex(x => x.Nombre).IsUnique();
        }
    }

    public class UsuarioRolConfiguration : IEntityTypeConfiguration<UsuarioRol>
    {
        public void Configure(EntityTypeBuilder<UsuarioRol> builder)
        {
            builder.HasKey(x => new { x.IdUsuarios, x.IdRoles });

            builder.HasOne(x => x.Usuario)
                .WithMany(x => x.UsuariosRoles)
                .HasForeignKey(x => x.IdUsuarios);

            builder.HasOne(x => x.Rol)
                .WithMany(x => x.UsuariosRoles)
                .HasForeignKey(x => x.IdRoles);
        }
    }

    public class RolPermisoConfiguration : IEntityTypeConfiguration<RolPermiso>
    {
        public void Configure(EntityTypeBuilder<RolPermiso> builder)
        {
            builder.HasKey(x => new { x.IdRoles, x.IdPermisos });

            builder.HasOne(x => x.Rol)
                .WithMany(x => x.RolesPermisos)
                .HasForeignKey(x => x.IdRoles);

            builder.HasOne(x => x.Permiso)
                .WithMany(x => x.RolesPermisos)
                .HasForeignKey(x => x.IdPermisos);
        }
    }
}
