// src/modules/pilotlicense/Infrastructure/entity/PilotLicenseEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.pilotlicense.Infrastructure.entity;

public class PilotLicenseEntityConfig : IEntityTypeConfiguration<PilotLicenseEntity>
{
    public void Configure(EntityTypeBuilder<PilotLicenseEntity> builder)
    {
        builder.ToTable("licencias_piloto");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(l => l.TrabajadorId).HasColumnName("trabajador_id").IsRequired();
        builder.Property(l => l.NumeroLicencia).HasColumnName("numero_licencia").HasMaxLength(50).IsRequired();
        builder.Property(l => l.TipoLicencia).HasColumnName("tipo_licencia").HasMaxLength(4).IsRequired();
        builder.Property(l => l.FechaExpedicion).HasColumnName("fecha_expedicion").IsRequired();
        builder.Property(l => l.FechaVencimiento).HasColumnName("fecha_vencimiento").IsRequired();
        builder.Property(l => l.AutoridadEmisora).HasColumnName("autoridad_emisora").HasMaxLength(100).IsRequired();
        builder.Property(l => l.Activa).HasColumnName("activa").IsRequired().HasDefaultValue(true);

        builder.HasIndex(l => l.NumeroLicencia).IsUnique();
        builder.ToTable(t => t.HasCheckConstraint("chk_tipo_licencia",
            "tipo_licencia IN ('PPL','CPL','ATPL')"));

        builder.HasOne(l => l.Trabajador)
            .WithMany(w => w.Licencias)
            .HasForeignKey(l => l.TrabajadorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}