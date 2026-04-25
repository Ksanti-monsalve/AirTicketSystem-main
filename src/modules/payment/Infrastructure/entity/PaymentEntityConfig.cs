// src/modules/payment/Infrastructure/entity/PaymentEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.payment.Infrastructure.entity;

public class PaymentEntityConfig : IEntityTypeConfiguration<PaymentEntity>
{
    public void Configure(EntityTypeBuilder<PaymentEntity> builder)
    {
        builder.ToTable("pagos");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(p => p.ReservaId).HasColumnName("reserva_id").IsRequired();
        builder.Property(p => p.MetodoPagoId).HasColumnName("metodo_pago_id").IsRequired();
        builder.Property(p => p.Monto).HasColumnName("monto").HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(p => p.Estado).HasColumnName("estado").HasMaxLength(12)
            .IsRequired().HasDefaultValue("PENDIENTE");
        builder.Property(p => p.ReferenciaPago).HasColumnName("referencia_pago").HasMaxLength(100);
        builder.Property(p => p.FechaPago).HasColumnName("fecha_pago");
        builder.Property(p => p.FechaVencimiento).HasColumnName("fecha_vencimiento");

        builder.ToTable(t => t.HasCheckConstraint("chk_estado_pago",
            "estado IN ('PENDIENTE','APROBADO','RECHAZADO','REEMBOLSADO')"));

        builder.HasOne(p => p.Reserva)
            .WithMany(b => b.Pagos)
            .HasForeignKey(p => p.ReservaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.MetodoPago)
            .WithMany(mp => mp.Pagos)
            .HasForeignKey(p => p.MetodoPagoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}