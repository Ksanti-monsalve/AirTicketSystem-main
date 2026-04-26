// src/modules/seatavailability/Infrastructure/entity/SeatAvailabilityEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AirTicketSystem.modules.booking.Infrastructure.entity;
using AirTicketSystem.modules.ticket.Infrastructure.entity;

namespace AirTicketSystem.modules.seatavailability.Infrastructure.entity;

public class SeatAvailabilityEntityConfig : IEntityTypeConfiguration<SeatAvailabilityEntity>
{
    public void Configure(EntityTypeBuilder<SeatAvailabilityEntity> builder)
    {
        builder.ToTable("disponibilidad_asientos");

        builder.HasKey(sa => sa.Id);
        builder.Property(sa => sa.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(sa => sa.VueloId).HasColumnName("vuelo_id").IsRequired();
        builder.Property(sa => sa.AsientoId).HasColumnName("asiento_id").IsRequired();
        builder.Property(sa => sa.NumeroAsiento).HasColumnName("numero_asiento").HasMaxLength(5).IsRequired();
        builder.Property(sa => sa.ClaseVueloId).HasColumnName("clase_vuelo_id").IsRequired();
        builder.Property(sa => sa.Estado).HasColumnName("estado").HasMaxLength(15)
            .IsRequired().HasDefaultValue("DISPONIBLE");
        builder.Property(sa => sa.ReservaId).HasColumnName("reserva_id");
        builder.Property(sa => sa.TiqueteId).HasColumnName("tiquete_id");

        // Requisito EXAMEN: Número de asiento único por vuelo
        builder.HasIndex(sa => new { sa.VueloId, sa.NumeroAsiento }).IsUnique();
        // Mantener unicidad técnica (por asiento físico)
        builder.HasIndex(sa => new { sa.VueloId, sa.AsientoId }).IsUnique();
        builder.HasIndex(sa => sa.ReservaId);
        builder.HasIndex(sa => sa.TiqueteId);
        builder.ToTable(t => t.HasCheckConstraint("chk_estado_asiento",
            "estado IN ('DISPONIBLE','RESERVADO','OCUPADO','BLOQUEADO')"));

        builder.HasOne(sa => sa.Vuelo)
            .WithMany(f => f.DisponibilidadAsientos)
            .HasForeignKey(sa => sa.VueloId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sa => sa.Asiento)
            .WithMany(a => a.Disponibilidades)
            .HasForeignKey(sa => sa.AsientoId)
            .OnDelete(DeleteBehavior.Restrict);

        // IMPORTANTE: usar navegación explícita para evitar columnas sombra (ReservaId1)
        builder.HasOne(sa => sa.Reserva)
            .WithMany()
            .HasForeignKey(sa => sa.ReservaId)
            .OnDelete(DeleteBehavior.SetNull);

        // IMPORTANTE: usar navegación explícita para evitar columnas sombra (TiqueteId1)
        builder.HasOne(sa => sa.Tiquete)
            .WithMany()
            .HasForeignKey(sa => sa.TiqueteId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}