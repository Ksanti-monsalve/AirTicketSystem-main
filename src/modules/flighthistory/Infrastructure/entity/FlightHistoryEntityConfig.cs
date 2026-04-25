// src/modules/flighthistory/Infrastructure/entity/FlightHistoryEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.flighthistory.Infrastructure.entity;

public class FlightHistoryEntityConfig : IEntityTypeConfiguration<FlightHistoryEntity>
{
    public void Configure(EntityTypeBuilder<FlightHistoryEntity> builder)
    {
        builder.ToTable("historial_vuelo");

        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(h => h.VueloId).HasColumnName("vuelo_id").IsRequired();
        builder.Property(h => h.EstadoAnterior).HasColumnName("estado_anterior").HasMaxLength(50).IsRequired();
        builder.Property(h => h.EstadoNuevo).HasColumnName("estado_nuevo").HasMaxLength(50).IsRequired();
        builder.Property(h => h.FechaCambio).HasColumnName("fecha_cambio").IsRequired();
        builder.Property(h => h.UsuarioId).HasColumnName("usuario_id");
        builder.Property(h => h.Motivo).HasColumnName("motivo").HasMaxLength(300);

        builder.HasOne(h => h.Vuelo)
            .WithMany(f => f.Historial)
            .HasForeignKey(h => h.VueloId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(h => h.Usuario)
            .WithMany()
            .HasForeignKey(h => h.UsuarioId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}