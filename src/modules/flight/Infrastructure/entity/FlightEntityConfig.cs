// src/modules/flight/Infrastructure/entity/FlightEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.flight.Infrastructure.entity;

public class FlightEntityConfig : IEntityTypeConfiguration<FlightEntity>
{
    public void Configure(EntityTypeBuilder<FlightEntity> builder)
    {
        builder.ToTable("vuelos");

        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(f => f.NumeroVuelo).HasColumnName("numero_vuelo").HasMaxLength(10).IsRequired();
        builder.Property(f => f.RutaId).HasColumnName("ruta_id").IsRequired();
        builder.Property(f => f.AvionId).HasColumnName("avion_id").IsRequired();
        builder.Property(f => f.PuertaEmbarqueId).HasColumnName("puerta_embarque_id");
        builder.Property(f => f.FechaSalida).HasColumnName("fecha_salida").IsRequired();
        builder.Property(f => f.FechaLlegadaEstimada).HasColumnName("fecha_llegada_estimada").IsRequired();
        builder.Property(f => f.FechaLlegadaReal).HasColumnName("fecha_llegada_real");
        builder.Property(f => f.Estado).HasColumnName("estado").HasMaxLength(20)
            .IsRequired().HasDefaultValue("PROGRAMADO");
        builder.Property(f => f.MotivoCambioEstado).HasColumnName("motivo_cambio_estado").HasMaxLength(300);
        builder.Property(f => f.CheckinApertura).HasColumnName("checkin_apertura");
        builder.Property(f => f.CheckinCierre).HasColumnName("checkin_cierre");

        builder.HasIndex(f => new { f.NumeroVuelo, f.FechaSalida }).IsUnique();
        builder.ToTable(t => t.HasCheckConstraint("chk_estado_vuelo",
            "estado IN ('PROGRAMADO','ABORDANDO','EN_VUELO','ATERRIZADO','CANCELADO','DEMORADO','DESVIADO')"));

        builder.HasOne(f => f.Ruta)
            .WithMany(r => r.Vuelos)
            .HasForeignKey(f => f.RutaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Avion)
            .WithMany(a => a.Vuelos)
            .HasForeignKey(f => f.AvionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.PuertaEmbarque)
            .WithMany(g => g.Vuelos)
            .HasForeignKey(f => f.PuertaEmbarqueId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}