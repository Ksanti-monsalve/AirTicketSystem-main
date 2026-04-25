// src/modules/airline/Infrastructure/entity/AirlinePhoneEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.airline.Infrastructure.entity;

public class AirlinePhoneEntityConfig : IEntityTypeConfiguration<AirlinePhoneEntity>
{
    public void Configure(EntityTypeBuilder<AirlinePhoneEntity> builder)
    {
        builder.ToTable("telefonos_aerolinea");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(p => p.AerolineaId).HasColumnName("aerolinea_id").IsRequired();
        builder.Property(p => p.TipoTelefonoId).HasColumnName("tipo_telefono_id").IsRequired();
        builder.Property(p => p.Numero).HasColumnName("numero").HasMaxLength(20).IsRequired();
        builder.Property(p => p.IndicativoPais).HasColumnName("indicativo_pais").HasMaxLength(5);
        builder.Property(p => p.EsPrincipal).HasColumnName("es_principal").IsRequired().HasDefaultValue(false);

        builder.HasIndex(p => new { p.AerolineaId, p.Numero }).IsUnique();

        builder.HasOne(p => p.Aerolinea)
            .WithMany(a => a.Telefonos)
            .HasForeignKey(p => p.AerolineaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.TipoTelefono)
            .WithMany(t => t.TelefonosAerolinea)
            .HasForeignKey(p => p.TipoTelefonoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}