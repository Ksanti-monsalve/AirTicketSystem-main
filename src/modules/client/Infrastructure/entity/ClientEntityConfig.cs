// src/modules/client/Infrastructure/entity/ClientEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.client.Infrastructure.entity;

public class ClientEntityConfig : IEntityTypeConfiguration<ClientEntity>
{
    public void Configure(EntityTypeBuilder<ClientEntity> builder)
    {
        builder.ToTable("clientes");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(c => c.PersonaId).HasColumnName("persona_id").IsRequired();
        builder.Property(c => c.UsuarioId).HasColumnName("usuario_id").IsRequired();
        builder.Property(c => c.Activo).HasColumnName("activo").IsRequired().HasDefaultValue(true);
        builder.Property(c => c.FechaRegistro).HasColumnName("fecha_registro").IsRequired();

        builder.HasIndex(c => c.PersonaId).IsUnique();
        builder.HasIndex(c => c.UsuarioId).IsUnique();

        builder.HasOne(c => c.Persona)
            .WithOne(p => p.Cliente)
            .HasForeignKey<ClientEntity>(c => c.PersonaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Usuario)
            .WithOne(u => u.Cliente)
            .HasForeignKey<ClientEntity>(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}