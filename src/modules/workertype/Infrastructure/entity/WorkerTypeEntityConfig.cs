// src/modules/workertype/Infrastructure/entity/WorkerTypeEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.workertype.Infrastructure.entity;

public class WorkerTypeEntityConfig : IEntityTypeConfiguration<WorkerTypeEntity>
{
    public void Configure(EntityTypeBuilder<WorkerTypeEntity> builder)
    {
        builder.ToTable("tipos_trabajador");
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(w => w.Nombre).HasColumnName("nombre").HasMaxLength(50).IsRequired();
        builder.HasIndex(w => w.Nombre).IsUnique();
    }
}