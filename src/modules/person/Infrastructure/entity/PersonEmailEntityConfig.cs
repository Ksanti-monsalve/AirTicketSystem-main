// src/modules/person/Infrastructure/entity/PersonEmailEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.person.Infrastructure.entity;

public class PersonEmailEntityConfig : IEntityTypeConfiguration<PersonEmailEntity>
{
    public void Configure(EntityTypeBuilder<PersonEmailEntity> builder)
    {
        builder.ToTable("emails_persona");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(e => e.PersonaId).HasColumnName("persona_id").IsRequired();
        builder.Property(e => e.TipoEmailId).HasColumnName("tipo_email_id").IsRequired();
        builder.Property(e => e.Email).HasColumnName("email").HasMaxLength(150).IsRequired();
        builder.Property(e => e.EsPrincipal).HasColumnName("es_principal").IsRequired().HasDefaultValue(false);

        builder.HasIndex(e => e.Email).IsUnique();

        builder.HasOne(e => e.Persona)
            .WithMany(p => p.Emails)
            .HasForeignKey(e => e.PersonaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.TipoEmail)
            .WithMany(t => t.EmailsPersona)
            .HasForeignKey(e => e.TipoEmailId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}