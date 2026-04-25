// src/modules/paymentmethod/Infrastructure/entity/PaymentMethodEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.paymentmethod.Infrastructure.entity;

public class PaymentMethodEntityConfig : IEntityTypeConfiguration<PaymentMethodEntity>
{
    public void Configure(EntityTypeBuilder<PaymentMethodEntity> builder)
    {
        builder.ToTable("metodos_pago");
        builder.HasKey(pm => pm.Id);
        builder.Property(pm => pm.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(pm => pm.Nombre).HasColumnName("nombre").HasMaxLength(50).IsRequired();
        builder.HasIndex(pm => pm.Nombre).IsUnique();
    }
}