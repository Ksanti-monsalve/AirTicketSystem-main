// src/modules/specialty/Infrastructure/entity/SpecialtyEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.specialty.Infrastructure.entity;

public class SpecialtyEntityConfig : IEntityTypeConfiguration<SpecialtyEntity>
{
    public void Configure(EntityTypeBuilder<SpecialtyEntity> builder)
    {
        builder.ToTable("especialidades");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(s => s.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
        builder.Property(s => s.TipoTrabajadorId).HasColumnName("tipo_trabajador_id");
        builder.HasIndex(s => s.Nombre).IsUnique();

        builder.HasOne(s => s.TipoTrabajador)
            .WithMany(t => t.Especialidades)
            .HasForeignKey(s => s.TipoTrabajadorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}