// src/modules/luggage/Infrastructure/entity/LuggageEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.luggage.Infrastructure.entity;

public class LuggageEntityConfig : IEntityTypeConfiguration<LuggageEntity>
{
    public void Configure(EntityTypeBuilder<LuggageEntity> builder)
    {
        builder.ToTable("equipaje_registrado");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(l => l.PasajeroReservaId).HasColumnName("pasajero_reserva_id").IsRequired();
        builder.Property(l => l.VueloId).HasColumnName("vuelo_id").IsRequired();
        builder.Property(l => l.TipoEquipajeId).HasColumnName("tipo_equipaje_id").IsRequired();
        builder.Property(l => l.Descripcion).HasColumnName("descripcion").HasMaxLength(200);

        builder.Property(l => l.PesoDeclaradoKg).HasColumnName("peso_declarado_kg").HasColumnType("decimal(5,2)");
        builder.Property(l => l.LargoDeclaradoCm).HasColumnName("largo_declarado_cm");
        builder.Property(l => l.AnchoDeclaradoCm).HasColumnName("ancho_declarado_cm");
        builder.Property(l => l.AltoDeclaradoCm).HasColumnName("alto_declarado_cm");

        builder.Property(l => l.PesoRealKg).HasColumnName("peso_real_kg").HasColumnType("decimal(5,2)");
        builder.Property(l => l.LargoRealCm).HasColumnName("largo_real_cm");
        builder.Property(l => l.AnchoRealCm).HasColumnName("ancho_real_cm");
        builder.Property(l => l.AltoRealCm).HasColumnName("alto_real_cm");

        builder.Property(l => l.CodigoEquipaje).HasColumnName("codigo_equipaje").HasMaxLength(20);
        builder.Property(l => l.CostoAdicional).HasColumnName("costo_adicional")
            .HasColumnType("decimal(10,2)").IsRequired().HasDefaultValue(0m);
        builder.Property(l => l.Estado).HasColumnName("estado").HasMaxLength(15)
            .IsRequired().HasDefaultValue("DECLARADO");

        builder.HasIndex(l => l.CodigoEquipaje).IsUnique().HasFilter("codigo_equipaje IS NOT NULL");
        builder.ToTable(t => t.HasCheckConstraint("chk_estado_equipaje",
            "estado IN ('DECLARADO','REGISTRADO','EN_BODEGA','EN_DESTINO','ENTREGADO','PERDIDO','DAÑADO')"));

        builder.HasOne(l => l.PasajeroReserva)
            .WithMany(bp => bp.Equipajes)
            .HasForeignKey(l => l.PasajeroReservaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Vuelo)
            .WithMany(f => f.Equipajes)
            .HasForeignKey(l => l.VueloId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.TipoEquipaje)
            .WithMany(lt => lt.Equipajes)
            .HasForeignKey(l => l.TipoEquipajeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}