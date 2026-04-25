// src/modules/user/Infrastructure/entity/AccessLogEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.user.Infrastructure.entity;

public class AccessLogEntityConfig : IEntityTypeConfiguration<AccessLogEntity>
{
    public void Configure(EntityTypeBuilder<AccessLogEntity> builder)
    {
        builder.ToTable("log_acceso");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(l => l.UsuarioId).HasColumnName("usuario_id").IsRequired();
        builder.Property(l => l.FechaAcceso).HasColumnName("fecha_acceso").IsRequired();
        builder.Property(l => l.Tipo).HasColumnName("tipo").HasMaxLength(20).IsRequired();
        builder.Property(l => l.IpAddress).HasColumnName("ip_address").HasMaxLength(45);

        builder.HasOne(l => l.Usuario)
            .WithMany(u => u.LogsAcceso)
            .HasForeignKey(l => l.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}