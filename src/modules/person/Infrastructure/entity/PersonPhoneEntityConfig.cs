// src/modules/person/Infrastructure/entity/PersonPhoneEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.person.Infrastructure.entity;

public class PersonPhoneEntityConfig : IEntityTypeConfiguration<PersonPhoneEntity>
{
    public void Configure(EntityTypeBuilder<PersonPhoneEntity> builder)
    {
        builder.ToTable("telefonos_persona");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(p => p.PersonaId).HasColumnName("persona_id").IsRequired();
        builder.Property(p => p.TipoTelefonoId).HasColumnName("tipo_telefono_id").IsRequired();
        builder.Property(p => p.Numero).HasColumnName("numero").HasMaxLength(20).IsRequired();
        builder.Property(p => p.IndicativoPais).HasColumnName("indicativo_pais").HasMaxLength(5);
        builder.Property(p => p.EsPrincipal).HasColumnName("es_principal").IsRequired().HasDefaultValue(false);

        builder.HasIndex(p => new { p.PersonaId, p.Numero }).IsUnique();

        builder.HasOne(p => p.Persona)
            .WithMany(pe => pe.Telefonos)
            .HasForeignKey(p => p.PersonaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.TipoTelefono)
            .WithMany(t => t.TelefonosPersona)
            .HasForeignKey(p => p.TipoTelefonoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}