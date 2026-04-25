// src/modules/region/Infrastructure/entity/RegionEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.region.Infrastructure.entity;

public class RegionEntityConfig : IEntityTypeConfiguration<RegionEntity>
{
    public void Configure(EntityTypeBuilder<RegionEntity> builder)
    {
        builder.ToTable("regiones");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(r => r.PaisId).HasColumnName("pais_id").IsRequired();
        builder.Property(r => r.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
        builder.Property(r => r.Codigo).HasColumnName("codigo").HasMaxLength(10);

        builder.HasIndex(r => new { r.PaisId, r.Nombre }).IsUnique();

        builder.HasOne(r => r.Pais)
            .WithMany(p => p.Regiones)
            .HasForeignKey(r => r.PaisId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}