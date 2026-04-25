// src/modules/bookinghistory/Infrastructure/entity/BookingHistoryEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.bookinghistory.Infrastructure.entity;

public class BookingHistoryEntityConfig : IEntityTypeConfiguration<BookingHistoryEntity>
{
    public void Configure(EntityTypeBuilder<BookingHistoryEntity> builder)
    {
        builder.ToTable("historial_reserva");

        builder.HasKey(bh => bh.Id);
        builder.Property(bh => bh.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(bh => bh.ReservaId).HasColumnName("reserva_id").IsRequired();
        builder.Property(bh => bh.EstadoAnterior).HasColumnName("estado_anterior").HasMaxLength(50).IsRequired();
        builder.Property(bh => bh.EstadoNuevo).HasColumnName("estado_nuevo").HasMaxLength(50).IsRequired();
        builder.Property(bh => bh.FechaCambio).HasColumnName("fecha_cambio").IsRequired();
        builder.Property(bh => bh.UsuarioId).HasColumnName("usuario_id");
        builder.Property(bh => bh.Motivo).HasColumnName("motivo").HasMaxLength(300);

        builder.HasOne(bh => bh.Reserva)
            .WithMany(b => b.Historial)
            .HasForeignKey(bh => bh.ReservaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(bh => bh.Usuario)
            .WithMany()
            .HasForeignKey(bh => bh.UsuarioId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}