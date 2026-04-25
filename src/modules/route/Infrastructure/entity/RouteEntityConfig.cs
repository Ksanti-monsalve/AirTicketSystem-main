// src/modules/route/Infrastructure/entity/RouteEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.route.Infrastructure.entity;

public class RouteEntityConfig : IEntityTypeConfiguration<RouteEntity>
{
    public void Configure(EntityTypeBuilder<RouteEntity> builder)
    {
        builder.ToTable("rutas");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(r => r.AerolineaId).HasColumnName("aerolinea_id").IsRequired();
        builder.Property(r => r.OrigenId).HasColumnName("origen_id").IsRequired();
        builder.Property(r => r.DestinoId).HasColumnName("destino_id").IsRequired();
        builder.Property(r => r.DistanciaKm).HasColumnName("distancia_km");
        builder.Property(r => r.DuracionEstimadaMin).HasColumnName("duracion_estimada_min");
        builder.Property(r => r.Activa).HasColumnName("activa").IsRequired().HasDefaultValue(true);

        builder.HasIndex(r => new { r.AerolineaId, r.OrigenId, r.DestinoId }).IsUnique();
        builder.ToTable(t => t.HasCheckConstraint("chk_origen_destino_ruta", "origen_id <> destino_id"));

        builder.HasOne(r => r.Aerolinea)
            .WithMany(a => a.Rutas)
            .HasForeignKey(r => r.AerolineaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Origen)
            .WithMany(a => a.RutasOrigen)
            .HasForeignKey(r => r.OrigenId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Destino)
            .WithMany(a => a.RutasDestino)
            .HasForeignKey(r => r.DestinoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}