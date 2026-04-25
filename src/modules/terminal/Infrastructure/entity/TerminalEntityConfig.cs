// src/modules/terminal/Infrastructure/entity/TerminalEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.terminal.Infrastructure.entity;

public class TerminalEntityConfig : IEntityTypeConfiguration<TerminalEntity>
{
    public void Configure(EntityTypeBuilder<TerminalEntity> builder)
    {
        builder.ToTable("terminales");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(t => t.AeropuertoId).HasColumnName("aeropuerto_id").IsRequired();
        builder.Property(t => t.Nombre).HasColumnName("nombre").HasMaxLength(50).IsRequired();
        builder.Property(t => t.Descripcion).HasColumnName("descripcion").HasMaxLength(200);

        builder.HasIndex(t => new { t.AeropuertoId, t.Nombre }).IsUnique();

        builder.HasOne(t => t.Aeropuerto)
            .WithMany(a => a.Terminales)
            .HasForeignKey(t => t.AeropuertoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}