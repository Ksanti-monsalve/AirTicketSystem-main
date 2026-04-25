// src/modules/seatavailability/Infrastructure/entity/SeatAvailabilityEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
        builder.Property(sa => sa.Estado).HasColumnName("estado").HasMaxLength(15)
            .IsRequired().HasDefaultValue("DISPONIBLE");

        builder.HasIndex(sa => new { sa.VueloId, sa.AsientoId }).IsUnique();
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
    }
}