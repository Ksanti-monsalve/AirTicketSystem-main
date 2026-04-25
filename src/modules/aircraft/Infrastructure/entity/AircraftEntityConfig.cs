// src/modules/aircraft/Infrastructure/entity/AircraftEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.aircraft.Infrastructure.entity;

public class AircraftEntityConfig : IEntityTypeConfiguration<AircraftEntity>
{
    public void Configure(EntityTypeBuilder<AircraftEntity> builder)
    {
        builder.ToTable("aviones");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(a => a.Matricula).HasColumnName("matricula").HasMaxLength(20).IsRequired();
        builder.Property(a => a.ModeloAvionId).HasColumnName("modelo_avion_id").IsRequired();
        builder.Property(a => a.AerolineaId).HasColumnName("aerolinea_id").IsRequired();
        builder.Property(a => a.FechaFabricacion).HasColumnName("fecha_fabricacion");
        builder.Property(a => a.FechaUltimoMantenimiento).HasColumnName("fecha_ultimo_mantenimiento");
        builder.Property(a => a.FechaProximoMantenimiento).HasColumnName("fecha_proximo_mantenimiento");
        builder.Property(a => a.TotalHorasVuelo).HasColumnName("total_horas_vuelo")
            .HasColumnType("decimal(10,2)").IsRequired().HasDefaultValue(0m);
        builder.Property(a => a.Estado).HasColumnName("estado").HasMaxLength(20)
            .IsRequired().HasDefaultValue("DISPONIBLE");
        builder.Property(a => a.Activo).HasColumnName("activo").IsRequired().HasDefaultValue(true);

        builder.HasIndex(a => a.Matricula).IsUnique();
        builder.ToTable(t => t.HasCheckConstraint("chk_estado_avion",
            "estado IN ('DISPONIBLE','EN_VUELO','MANTENIMIENTO','FUERA_DE_SERVICIO')"));

        builder.HasOne(a => a.ModeloAvion)
            .WithMany(m => m.Aviones)
            .HasForeignKey(a => a.ModeloAvionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Aerolinea)
            .WithMany(al => al.Aviones)
            .HasForeignKey(a => a.AerolineaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}