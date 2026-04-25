// src/modules/worker/Infrastructure/entity/WorkerSpecialtyEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.worker.Infrastructure.entity;

public class WorkerSpecialtyEntityConfig : IEntityTypeConfiguration<WorkerSpecialtyEntity>
{
    public void Configure(EntityTypeBuilder<WorkerSpecialtyEntity> builder)
    {
        builder.ToTable("trabajador_especialidades");

        builder.HasKey(ws => ws.Id);
        builder.Property(ws => ws.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(ws => ws.TrabajadorId).HasColumnName("trabajador_id").IsRequired();
        builder.Property(ws => ws.EspecialidadId).HasColumnName("especialidad_id").IsRequired();

        builder.HasIndex(ws => new { ws.TrabajadorId, ws.EspecialidadId }).IsUnique();

        builder.HasOne(ws => ws.Trabajador)
            .WithMany(w => w.Especialidades)
            .HasForeignKey(ws => ws.TrabajadorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ws => ws.Especialidad)
            .WithMany(e => e.TrabajadorEspecialidades)
            .HasForeignKey(ws => ws.EspecialidadId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}