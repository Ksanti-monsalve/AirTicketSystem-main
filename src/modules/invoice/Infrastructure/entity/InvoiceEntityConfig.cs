// src/modules/invoice/Infrastructure/entity/InvoiceEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.invoice.Infrastructure.entity;

public class InvoiceEntityConfig : IEntityTypeConfiguration<InvoiceEntity>
{
    public void Configure(EntityTypeBuilder<InvoiceEntity> builder)
    {
        builder.ToTable("facturas");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(i => i.ReservaId).HasColumnName("reserva_id").IsRequired();
        builder.Property(i => i.NumeroFactura).HasColumnName("numero_factura").HasMaxLength(30).IsRequired();
        builder.Property(i => i.FechaEmision).HasColumnName("fecha_emision").IsRequired();
        builder.Property(i => i.DireccionFacturacionId).HasColumnName("direccion_facturacion_id").IsRequired();
        builder.Property(i => i.Subtotal).HasColumnName("subtotal").HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(i => i.Impuestos).HasColumnName("impuestos").HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(i => i.Total).HasColumnName("total").HasColumnType("decimal(12,2)").IsRequired();

        builder.HasIndex(i => i.ReservaId).IsUnique();
        builder.HasIndex(i => i.NumeroFactura).IsUnique();

        builder.HasOne(i => i.Reserva)
            .WithOne(b => b.Factura)
            .HasForeignKey<InvoiceEntity>(i => i.ReservaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.DireccionFacturacion)
            .WithMany(a => a.Facturas)
            .HasForeignKey(i => i.DireccionFacturacionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}