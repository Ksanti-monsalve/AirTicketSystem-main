// src/modules/person/Infrastructure/entity/PersonEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.person.Infrastructure.entity;

public class PersonEntityConfig : IEntityTypeConfiguration<PersonEntity>
{
    public void Configure(EntityTypeBuilder<PersonEntity> builder)
    {
        builder.ToTable("personas");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(p => p.TipoDocId).HasColumnName("tipo_doc_id").IsRequired();
        builder.Property(p => p.NumeroDoc).HasColumnName("numero_doc").HasMaxLength(20).IsRequired();
        builder.Property(p => p.Nombres).HasColumnName("nombres").HasMaxLength(100).IsRequired();
        builder.Property(p => p.Apellidos).HasColumnName("apellidos").HasMaxLength(100).IsRequired();
        builder.Property(p => p.FechaNacimiento).HasColumnName("fecha_nacimiento");
        builder.Property(p => p.GeneroId).HasColumnName("genero_id");
        builder.Property(p => p.NacionalidadId).HasColumnName("nacionalidad_id");

        builder.HasIndex(p => new { p.TipoDocId, p.NumeroDoc }).IsUnique();

        builder.HasOne(p => p.TipoDocumento)
            .WithMany(t => t.Personas)
            .HasForeignKey(p => p.TipoDocId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Genero)
            .WithMany(g => g.Personas)
            .HasForeignKey(p => p.GeneroId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Nacionalidad)
            .WithMany(c => c.Personas)
            .HasForeignKey(p => p.NacionalidadId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}