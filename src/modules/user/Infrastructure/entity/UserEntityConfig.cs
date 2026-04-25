// src/modules/user/Infrastructure/entity/UserEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.user.Infrastructure.entity;

public class UserEntityConfig : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("usuarios");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(u => u.PersonaId).HasColumnName("persona_id").IsRequired();
        builder.Property(u => u.RolId).HasColumnName("rol_id").IsRequired();
        builder.Property(u => u.Username).HasColumnName("username").HasMaxLength(50).IsRequired();
        builder.Property(u => u.PasswordHash).HasColumnName("password_hash").HasMaxLength(255).IsRequired();
        builder.Property(u => u.Activo).HasColumnName("activo").IsRequired().HasDefaultValue(true);
        builder.Property(u => u.FechaRegistro).HasColumnName("fecha_registro").IsRequired();
        builder.Property(u => u.UltimoLogin).HasColumnName("ultimo_login");
        builder.Property(u => u.IntentosFallidos).HasColumnName("intentos_fallidos").IsRequired().HasDefaultValue(0);

        builder.HasIndex(u => u.Username).IsUnique();

        builder.HasOne(u => u.Persona)
            .WithMany()
            .HasForeignKey(u => u.PersonaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Rol)
            .WithMany(r => r.Usuarios)
            .HasForeignKey(u => u.RolId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}