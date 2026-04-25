// src/modules/fare/Infrastructure/entity/FareEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.fare.Infrastructure.entity;

public class FareEntityConfig : IEntityTypeConfiguration<FareEntity>
{
    public void Configure(EntityTypeBuilder<FareEntity> builder)
    {
        builder.ToTable("tarifas");

        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(f => f.RutaId).HasColumnName("ruta_id").IsRequired();
        builder.Property(f => f.ClaseServicioId).HasColumnName("clase_servicio_id").IsRequired();
        builder.Property(f => f.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
        builder.Property(f => f.PrecioBase).HasColumnName("precio_base").HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(f => f.Impuestos).HasColumnName("impuestos").HasColumnType("decimal(12,2)")
            .IsRequired().HasDefaultValue(0m);
        builder.Property(f => f.PrecioTotal).HasColumnName("precio_total").HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(f => f.PermiteCambios).HasColumnName("permite_cambios").IsRequired().HasDefaultValue(false);
        builder.Property(f => f.PermiteReembolso).HasColumnName("permite_reembolso").IsRequired().HasDefaultValue(false);
        builder.Property(f => f.Activa).HasColumnName("activa").IsRequired().HasDefaultValue(true);
        builder.Property(f => f.VigenteHasta).HasColumnName("vigente_hasta");

        builder.HasIndex(f => new { f.RutaId, f.ClaseServicioId, f.Nombre }).IsUnique();

        builder.HasOne(f => f.Ruta)
            .WithMany(r => r.Tarifas)
            .HasForeignKey(f => f.RutaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.ClaseServicio)
            .WithMany(cs => cs.Tarifas)
            .HasForeignKey(f => f.ClaseServicioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}