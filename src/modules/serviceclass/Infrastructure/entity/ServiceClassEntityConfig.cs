// src/modules/serviceclass/Infrastructure/entity/ServiceClassEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.serviceclass.Infrastructure.entity;

public class ServiceClassEntityConfig : IEntityTypeConfiguration<ServiceClassEntity>
{
    public void Configure(EntityTypeBuilder<ServiceClassEntity> builder)
    {
        builder.ToTable("clases_servicio");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(s => s.Nombre).HasColumnName("nombre").HasMaxLength(50).IsRequired();
        builder.Property(s => s.Codigo).HasColumnName("codigo").HasMaxLength(3)
            .IsRequired().IsFixedLength();
        builder.Property(s => s.Descripcion).HasColumnName("descripcion").HasMaxLength(200);

        builder.HasIndex(s => s.Nombre).IsUnique();
        builder.HasIndex(s => s.Codigo).IsUnique();
    }
}