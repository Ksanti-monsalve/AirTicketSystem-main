// src/modules/airport/Infrastructure/entity/AirportEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.airport.Infrastructure.entity;

public class AirportEntityConfig : IEntityTypeConfiguration<AirportEntity>
{
    public void Configure(EntityTypeBuilder<AirportEntity> builder)
    {
        builder.ToTable("aeropuertos");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(a => a.CodigoIata).HasColumnName("codigo_iata").HasMaxLength(3).IsRequired().IsFixedLength();
        builder.Property(a => a.CodigoIcao).HasColumnName("codigo_icao").HasMaxLength(4).IsRequired().IsFixedLength();
        builder.Property(a => a.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
        builder.Property(a => a.CiudadId).HasColumnName("ciudad_id").IsRequired();
        builder.Property(a => a.Direccion).HasColumnName("direccion").HasMaxLength(200);
        builder.Property(a => a.Activo).HasColumnName("activo").IsRequired().HasDefaultValue(true);

        builder.HasIndex(a => a.CodigoIata).IsUnique();
        builder.HasIndex(a => a.CodigoIcao).IsUnique();

        builder.HasOne(a => a.Ciudad)
            .WithMany(c => c.Aeropuertos)
            .HasForeignKey(a => a.CiudadId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}