// src/modules/ticket/Infrastructure/entity/TicketEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.ticket.Infrastructure.entity;

public class TicketEntityConfig : IEntityTypeConfiguration<TicketEntity>
{
    public void Configure(EntityTypeBuilder<TicketEntity> builder)
    {
        builder.ToTable("tiquetes");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(t => t.PasajeroReservaId).HasColumnName("pasajero_reserva_id").IsRequired();
        builder.Property(t => t.CodigoTiquete).HasColumnName("codigo_tiquete").HasMaxLength(20).IsRequired();
        builder.Property(t => t.AsientoConfirmadoId).HasColumnName("asiento_confirmado_id");
        builder.Property(t => t.FechaEmision).HasColumnName("fecha_emision").IsRequired();
        builder.Property(t => t.Estado).HasColumnName("estado").HasMaxLength(15)
            .IsRequired().HasDefaultValue("EMITIDO");

        builder.HasIndex(t => t.PasajeroReservaId).IsUnique();
        builder.HasIndex(t => t.CodigoTiquete).IsUnique();
        builder.ToTable(t => t.HasCheckConstraint("chk_estado_tiquete",
            "estado IN ('EMITIDO','CHECKIN_HECHO','ABORDADO','USADO','ANULADO')"));

        builder.HasOne(t => t.PasajeroReserva)
            .WithOne(bp => bp.Tiquete)
            .HasForeignKey<TicketEntity>(t => t.PasajeroReservaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.AsientoConfirmado)
            .WithMany(a => a.Tiquetes)
            .HasForeignKey(t => t.AsientoConfirmadoId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}