// src/modules/worker/Infrastructure/entity/WorkerEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.worker.Infrastructure.entity;

public class WorkerEntityConfig : IEntityTypeConfiguration<WorkerEntity>
{
    public void Configure(EntityTypeBuilder<WorkerEntity> builder)
    {
        builder.ToTable("trabajadores");

        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(w => w.PersonaId).HasColumnName("persona_id").IsRequired();
        builder.Property(w => w.TipoTrabajadorId).HasColumnName("tipo_trabajador_id").IsRequired();
        builder.Property(w => w.AerolineaId).HasColumnName("aerolinea_id");
        builder.Property(w => w.AeropuertoBaseId).HasColumnName("aeropuerto_base_id").IsRequired();
        builder.Property(w => w.FechaContratacion).HasColumnName("fecha_contratacion").IsRequired();
        builder.Property(w => w.Salario).HasColumnName("salario").HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(w => w.Activo).HasColumnName("activo").IsRequired().HasDefaultValue(true);
        builder.Property(w => w.UsuarioId).HasColumnName("usuario_id");

        builder.HasIndex(w => w.PersonaId).IsUnique();

        builder.HasOne(w => w.Persona)
            .WithOne(p => p.Trabajador)
            .HasForeignKey<WorkerEntity>(w => w.PersonaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(w => w.TipoTrabajador)
            .WithMany(t => t.Trabajadores)
            .HasForeignKey(w => w.TipoTrabajadorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(w => w.Aerolinea)
            .WithMany(a => a.Trabajadores)
            .HasForeignKey(w => w.AerolineaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(w => w.AeropuertoBase)
            .WithMany(a => a.TrabajadoresBase)
            .HasForeignKey(w => w.AeropuertoBaseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(w => w.Usuario)
            .WithOne(u => u.Trabajador)
            .HasForeignKey<WorkerEntity>(w => w.UsuarioId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}