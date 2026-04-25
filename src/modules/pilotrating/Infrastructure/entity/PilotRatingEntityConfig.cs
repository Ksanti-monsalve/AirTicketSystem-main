// src/modules/pilotrating/Infrastructure/entity/PilotRatingEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.pilotrating.Infrastructure.entity;

public class PilotRatingEntityConfig : IEntityTypeConfiguration<PilotRatingEntity>
{
    public void Configure(EntityTypeBuilder<PilotRatingEntity> builder)
    {
        builder.ToTable("habilitaciones_piloto");

        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(h => h.LicenciaId).HasColumnName("licencia_id").IsRequired();
        builder.Property(h => h.ModeloAvionId).HasColumnName("modelo_avion_id").IsRequired();
        builder.Property(h => h.FechaHabilitacion).HasColumnName("fecha_habilitacion").IsRequired();
        builder.Property(h => h.FechaVencimiento).HasColumnName("fecha_vencimiento").IsRequired();

        builder.HasIndex(h => new { h.LicenciaId, h.ModeloAvionId }).IsUnique();

        builder.HasOne(h => h.Licencia)
            .WithMany(l => l.Habilitaciones)
            .HasForeignKey(h => h.LicenciaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(h => h.ModeloAvion)
            .WithMany(m => m.Habilitaciones)
            .HasForeignKey(h => h.ModeloAvionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}