// src/modules/aircraftmanufacturer/Infrastructure/entity/AircraftManufacturerEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.aircraftmanufacturer.Infrastructure.entity;

public class AircraftManufacturerEntityConfig : IEntityTypeConfiguration<AircraftManufacturerEntity>
{
    public void Configure(EntityTypeBuilder<AircraftManufacturerEntity> builder)
    {
        builder.ToTable("fabricantes_avion");

        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(f => f.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
        builder.Property(f => f.PaisId).HasColumnName("pais_id").IsRequired();
        builder.Property(f => f.SitioWeb).HasColumnName("sitio_web").HasMaxLength(200);

        builder.HasIndex(f => f.Nombre).IsUnique();

        builder.HasOne(f => f.Pais)
            .WithMany(p => p.Fabricantes)
            .HasForeignKey(f => f.PaisId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}