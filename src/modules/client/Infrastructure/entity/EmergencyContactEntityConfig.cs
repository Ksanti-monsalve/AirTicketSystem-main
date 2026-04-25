// src/modules/client/Infrastructure/entity/EmergencyContactEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.client.Infrastructure.entity;

public class EmergencyContactEntityConfig : IEntityTypeConfiguration<EmergencyContactEntity>
{
    public void Configure(EntityTypeBuilder<EmergencyContactEntity> builder)
    {
        builder.ToTable("contactos_emergencia");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(e => e.ClienteId).HasColumnName("cliente_id").IsRequired();
        builder.Property(e => e.PersonaId).HasColumnName("persona_id").IsRequired();
        builder.Property(e => e.RelacionId).HasColumnName("relacion_id").IsRequired();
        builder.Property(e => e.EsPrincipal).HasColumnName("es_principal").IsRequired().HasDefaultValue(false);

        builder.HasOne(e => e.Cliente)
            .WithMany(c => c.ContactosEmergencia)
            .HasForeignKey(e => e.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Persona)
            .WithMany(p => p.ContactosDeEmergencia)
            .HasForeignKey(e => e.PersonaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Relacion)
            .WithMany(r => r.Contactos)
            .HasForeignKey(e => e.RelacionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}