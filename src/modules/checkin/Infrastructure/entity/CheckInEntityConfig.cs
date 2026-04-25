// src/modules/checkin/Infrastructure/entity/CheckInEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.checkin.Infrastructure.entity;

public class CheckInEntityConfig : IEntityTypeConfiguration<CheckInEntity>
{
    public void Configure(EntityTypeBuilder<CheckInEntity> builder)
    {
        builder.ToTable("checkin");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(c => c.PasajeroReservaId).HasColumnName("pasajero_reserva_id").IsRequired();
        builder.Property(c => c.Tipo).HasColumnName("tipo").HasMaxLength(12).IsRequired();
        builder.Property(c => c.FechaCheckin).HasColumnName("fecha_checkin").IsRequired();
        builder.Property(c => c.TrabajadorId).HasColumnName("trabajador_id");
        builder.Property(c => c.Estado).HasColumnName("estado").HasMaxLength(10)
            .IsRequired().HasDefaultValue("PENDIENTE");

        builder.HasIndex(c => c.PasajeroReservaId).IsUnique();
        builder.ToTable(t => t.HasCheckConstraint("chk_tipo_checkin",
            "tipo IN ('VIRTUAL','PRESENCIAL')"));
        builder.ToTable(t => t.HasCheckConstraint("chk_estado_checkin",
            "estado IN ('COMPLETADO','PENDIENTE','CANCELADO')"));

        builder.HasOne(c => c.PasajeroReserva)
            .WithOne(bp => bp.CheckIn)
            .HasForeignKey<CheckInEntity>(c => c.PasajeroReservaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Trabajador)
            .WithMany(w => w.CheckInsAtendidos)
            .HasForeignKey(c => c.TrabajadorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}