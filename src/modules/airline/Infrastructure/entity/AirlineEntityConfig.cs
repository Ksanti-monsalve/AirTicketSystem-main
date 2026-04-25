// src/modules/airline/Infrastructure/entity/AirlineEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.airline.Infrastructure.entity;

public class AirlineEntityConfig : IEntityTypeConfiguration<AirlineEntity>
{
    public void Configure(EntityTypeBuilder<AirlineEntity> builder)
    {
        builder.ToTable("aerolineas");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(a => a.CodigoIata).HasColumnName("codigo_iata").HasMaxLength(2).IsRequired().IsFixedLength();
        builder.Property(a => a.CodigoIcao).HasColumnName("codigo_icao").HasMaxLength(3).IsRequired().IsFixedLength();
        builder.Property(a => a.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
        builder.Property(a => a.NombreComercial).HasColumnName("nombre_comercial").HasMaxLength(100);
        builder.Property(a => a.PaisId).HasColumnName("pais_id").IsRequired();
        builder.Property(a => a.SitioWeb).HasColumnName("sitio_web").HasMaxLength(200);
        builder.Property(a => a.Activa).HasColumnName("activa").IsRequired().HasDefaultValue(true);

        builder.HasIndex(a => a.CodigoIata).IsUnique();
        builder.HasIndex(a => a.CodigoIcao).IsUnique();

        builder.HasOne(a => a.Pais)
            .WithMany(p => p.Aerolineas)
            .HasForeignKey(a => a.PaisId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}