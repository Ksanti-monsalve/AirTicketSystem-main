// src/modules/luggagerestriction/Infrastructure/entity/LuggageRestrictionEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.luggagerestriction.Infrastructure.entity;

public class LuggageRestrictionEntityConfig : IEntityTypeConfiguration<LuggageRestrictionEntity>
{
    public void Configure(EntityTypeBuilder<LuggageRestrictionEntity> builder)
    {
        builder.ToTable("restricciones_equipaje");

        builder.HasKey(lr => lr.Id);
        builder.Property(lr => lr.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(lr => lr.TarifaId).HasColumnName("tarifa_id").IsRequired();
        builder.Property(lr => lr.TipoEquipajeId).HasColumnName("tipo_equipaje_id").IsRequired();
        builder.Property(lr => lr.PiezasIncluidas).HasColumnName("piezas_incluidas").IsRequired().HasDefaultValue(0);
        builder.Property(lr => lr.PesoMaximoKg).HasColumnName("peso_maximo_kg")
            .HasColumnType("decimal(5,2)").IsRequired();
        builder.Property(lr => lr.LargoMaxCm).HasColumnName("largo_max_cm");
        builder.Property(lr => lr.AnchoMaxCm).HasColumnName("ancho_max_cm");
        builder.Property(lr => lr.AltoMaxCm).HasColumnName("alto_max_cm");
        builder.Property(lr => lr.CostoExcesoKg).HasColumnName("costo_exceso_kg")
            .HasColumnType("decimal(10,2)").IsRequired().HasDefaultValue(0m);

        builder.HasIndex(lr => new { lr.TarifaId, lr.TipoEquipajeId }).IsUnique();

        builder.HasOne(lr => lr.Tarifa)
            .WithMany(f => f.RestriccionesEquipaje)
            .HasForeignKey(lr => lr.TarifaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(lr => lr.TipoEquipaje)
            .WithMany(lt => lt.Restricciones)
            .HasForeignKey(lr => lr.TipoEquipajeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}