// src/modules/additionalcharge/Infrastructure/entity/AdditionalChargeEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.additionalcharge.Infrastructure.entity;

public class AdditionalChargeEntityConfig : IEntityTypeConfiguration<AdditionalChargeEntity>
{
    public void Configure(EntityTypeBuilder<AdditionalChargeEntity> builder)
    {
        builder.ToTable("cargos_adicionales");

        builder.HasKey(ac => ac.Id);
        builder.Property(ac => ac.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(ac => ac.ReservaId).HasColumnName("reserva_id").IsRequired();
        builder.Property(ac => ac.Concepto).HasColumnName("concepto").HasMaxLength(150).IsRequired();
        builder.Property(ac => ac.Monto).HasColumnName("monto").HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(ac => ac.Fecha).HasColumnName("fecha").IsRequired();

        builder.HasOne(ac => ac.Reserva)
            .WithMany(b => b.CargosAdicionales)
            .HasForeignKey(ac => ac.ReservaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}