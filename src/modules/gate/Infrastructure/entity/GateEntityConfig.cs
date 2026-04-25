// src/modules/gate/Infrastructure/entity/GateEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.gate.Infrastructure.entity;

public class GateEntityConfig : IEntityTypeConfiguration<GateEntity>
{
    public void Configure(EntityTypeBuilder<GateEntity> builder)
    {
        builder.ToTable("puertas_embarque");

        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(g => g.TerminalId).HasColumnName("terminal_id").IsRequired();
        builder.Property(g => g.Codigo).HasColumnName("codigo").HasMaxLength(10).IsRequired();
        builder.Property(g => g.Activa).HasColumnName("activa").IsRequired().HasDefaultValue(true);

        builder.HasIndex(g => new { g.TerminalId, g.Codigo }).IsUnique();

        builder.HasOne(g => g.Terminal)
            .WithMany(t => t.Puertas)
            .HasForeignKey(g => g.TerminalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}