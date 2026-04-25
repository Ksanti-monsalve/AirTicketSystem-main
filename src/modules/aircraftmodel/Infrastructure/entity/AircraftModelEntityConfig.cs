// src/modules/aircraftmodel/Infrastructure/entity/AircraftModelEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.aircraftmodel.Infrastructure.entity;

public class AircraftModelEntityConfig : IEntityTypeConfiguration<AircraftModelEntity>
{
    public void Configure(EntityTypeBuilder<AircraftModelEntity> builder)
    {
        builder.ToTable("modelos_avion");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(m => m.FabricanteId).HasColumnName("fabricante_id").IsRequired();
        builder.Property(m => m.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
        builder.Property(m => m.CodigoModelo).HasColumnName("codigo_modelo").HasMaxLength(50).IsRequired();
        builder.Property(m => m.AutonomiKm).HasColumnName("autonomia_km");
        builder.Property(m => m.VelocidadCruceroKmh).HasColumnName("velocidad_crucero_kmh");
        builder.Property(m => m.Descripcion).HasColumnName("descripcion").HasMaxLength(300);

        builder.HasIndex(m => m.CodigoModelo).IsUnique();
        builder.HasIndex(m => new { m.FabricanteId, m.Nombre }).IsUnique();

        builder.HasOne(m => m.Fabricante)
            .WithMany(f => f.Modelos)
            .HasForeignKey(m => m.FabricanteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}