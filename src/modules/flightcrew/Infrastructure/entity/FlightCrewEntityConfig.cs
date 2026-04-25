// src/modules/flightcrew/Infrastructure/entity/FlightCrewEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.flightcrew.Infrastructure.entity;

public class FlightCrewEntityConfig : IEntityTypeConfiguration<FlightCrewEntity>
{
    public void Configure(EntityTypeBuilder<FlightCrewEntity> builder)
    {
        builder.ToTable("tripulacion_vuelo");

        builder.HasKey(fc => fc.Id);
        builder.Property(fc => fc.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(fc => fc.VueloId).HasColumnName("vuelo_id").IsRequired();
        builder.Property(fc => fc.TrabajadorId).HasColumnName("trabajador_id").IsRequired();
        builder.Property(fc => fc.RolEnVuelo).HasColumnName("rol_en_vuelo").HasMaxLength(20).IsRequired();

        // Un trabajador no puede tener dos roles en el mismo vuelo
        builder.HasIndex(fc => new { fc.VueloId, fc.TrabajadorId }).IsUnique();
        // Un rol no puede estar duplicado en el mismo vuelo
        builder.HasIndex(fc => new { fc.VueloId, fc.RolEnVuelo }).IsUnique();

        builder.ToTable(t => t.HasCheckConstraint("chk_rol_vuelo",
            "rol_en_vuelo IN ('PILOTO','COPILOTO','SOBRECARGO','AUXILIAR_VUELO','AUXILIAR_SEGURIDAD')"));

        builder.HasOne(fc => fc.Vuelo)
            .WithMany(f => f.Tripulacion)
            .HasForeignKey(fc => fc.VueloId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(fc => fc.Trabajador)
            .WithMany(w => w.Tripulaciones)
            .HasForeignKey(fc => fc.TrabajadorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}