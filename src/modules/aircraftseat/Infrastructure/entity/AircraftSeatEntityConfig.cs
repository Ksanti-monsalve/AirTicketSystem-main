// src/modules/aircraftseat/Infrastructure/entity/AircraftSeatEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.aircraftseat.Infrastructure.entity;

public class AircraftSeatEntityConfig : IEntityTypeConfiguration<AircraftSeatEntity>
{
    public void Configure(EntityTypeBuilder<AircraftSeatEntity> builder)
    {
        builder.ToTable("asientos_avion");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(a => a.AvionId).HasColumnName("avion_id").IsRequired();
        builder.Property(a => a.ClaseServicioId).HasColumnName("clase_servicio_id").IsRequired();
        builder.Property(a => a.CodigoAsiento).HasColumnName("codigo_asiento").HasMaxLength(5).IsRequired();
        builder.Property(a => a.Fila).HasColumnName("fila").IsRequired();
        builder.Property(a => a.Columna).HasColumnName("columna").HasMaxLength(1).IsRequired();
        builder.Property(a => a.EsVentana).HasColumnName("es_ventana").IsRequired().HasDefaultValue(false);
        builder.Property(a => a.EsPasillo).HasColumnName("es_pasillo").IsRequired().HasDefaultValue(false);
        builder.Property(a => a.Activo).HasColumnName("activo").IsRequired().HasDefaultValue(true);
        builder.Property(a => a.CostoSeleccion).HasColumnName("costo_seleccion").HasColumnType("decimal(10,2)").IsRequired().HasDefaultValue(0m);

        builder.HasIndex(a => new { a.AvionId, a.CodigoAsiento }).IsUnique();

        builder.HasOne(a => a.Avion)
            .WithMany(av => av.Asientos)
            .HasForeignKey(a => a.AvionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.ClaseServicio)
            .WithMany(cs => cs.Asientos)
            .HasForeignKey(a => a.ClaseServicioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}