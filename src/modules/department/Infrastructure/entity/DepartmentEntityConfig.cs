// src/modules/department/Infrastructure/entity/DepartmentEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.department.Infrastructure.entity;

public class DepartmentEntityConfig : IEntityTypeConfiguration<DepartmentEntity>
{
    public void Configure(EntityTypeBuilder<DepartmentEntity> builder)
    {
        builder.ToTable("departamentos_estados");

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(d => d.RegionId).HasColumnName("region_id").IsRequired();
        builder.Property(d => d.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
        builder.Property(d => d.Codigo).HasColumnName("codigo").HasMaxLength(10);

        builder.HasIndex(d => new { d.RegionId, d.Nombre }).IsUnique();

        builder.HasOne(d => d.Region)
            .WithMany(r => r.Departamentos)
            .HasForeignKey(d => d.RegionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
