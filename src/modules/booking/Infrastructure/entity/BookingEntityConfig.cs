// src/modules/booking/Infrastructure/entity/BookingEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.booking.Infrastructure.entity;

public class BookingEntityConfig : IEntityTypeConfiguration<BookingEntity>
{
    public void Configure(EntityTypeBuilder<BookingEntity> builder)
    {
        builder.ToTable("reservas");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(b => b.ClienteId).HasColumnName("cliente_id").IsRequired();
        builder.Property(b => b.VueloId).HasColumnName("vuelo_id").IsRequired();
        builder.Property(b => b.TarifaId).HasColumnName("tarifa_id").IsRequired();
        builder.Property(b => b.CodigoReserva).HasColumnName("codigo_reserva").HasMaxLength(10).IsRequired();
        builder.Property(b => b.FechaReserva).HasColumnName("fecha_reserva").IsRequired();
        builder.Property(b => b.FechaExpiracion).HasColumnName("fecha_expiracion").IsRequired();
        builder.Property(b => b.Estado).HasColumnName("estado").HasMaxLength(15)
            .IsRequired().HasDefaultValue("PENDIENTE");
        builder.Property(b => b.ValorTotal).HasColumnName("valor_total")
            .HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(b => b.Observaciones).HasColumnName("observaciones").HasMaxLength(300);

        builder.HasIndex(b => b.CodigoReserva).IsUnique();
        builder.ToTable(t => t.HasCheckConstraint("chk_estado_reserva",
            "estado IN ('PENDIENTE','CONFIRMADA','CANCELADA','EXPIRADA')"));

        builder.HasOne(b => b.Cliente)
            .WithMany(c => c.Reservas)
            .HasForeignKey(b => b.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Vuelo)
            .WithMany(f => f.Reservas)
            .HasForeignKey(b => b.VueloId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Tarifa)
            .WithMany(f => f.Reservas)
            .HasForeignKey(b => b.TarifaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}