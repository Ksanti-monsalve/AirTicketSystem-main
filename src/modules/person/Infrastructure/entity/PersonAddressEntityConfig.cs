// src/modules/person/Infrastructure/entity/PersonAddressEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.person.Infrastructure.entity;

public class PersonAddressEntityConfig : IEntityTypeConfiguration<PersonAddressEntity>
{
    public void Configure(EntityTypeBuilder<PersonAddressEntity> builder)
    {
        builder.ToTable("direcciones_persona");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(a => a.PersonaId).HasColumnName("persona_id").IsRequired();
        builder.Property(a => a.TipoDireccionId).HasColumnName("tipo_direccion_id").IsRequired();
        builder.Property(a => a.CiudadId).HasColumnName("ciudad_id").IsRequired();
        builder.Property(a => a.DireccionLinea1).HasColumnName("direccion_linea1").HasMaxLength(200).IsRequired();
        builder.Property(a => a.DireccionLinea2).HasColumnName("direccion_linea2").HasMaxLength(200);
        builder.Property(a => a.CodigoPostal).HasColumnName("codigo_postal").HasMaxLength(10);
        builder.Property(a => a.EsPrincipal).HasColumnName("es_principal").IsRequired().HasDefaultValue(false);

        builder.HasOne(a => a.Persona)
            .WithMany(p => p.Direcciones)
            .HasForeignKey(a => a.PersonaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.TipoDireccion)
            .WithMany(t => t.Direcciones)
            .HasForeignKey(a => a.TipoDireccionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Ciudad)
            .WithMany(c => c.Direcciones)
            .HasForeignKey(a => a.CiudadId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}