// src/modules/airline/Infrastructure/entity/AirlineEmailEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.airline.Infrastructure.entity;

public class AirlineEmailEntityConfig : IEntityTypeConfiguration<AirlineEmailEntity>
{
    public void Configure(EntityTypeBuilder<AirlineEmailEntity> builder)
    {
        builder.ToTable("emails_aerolinea");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(e => e.AerolineaId).HasColumnName("aerolinea_id").IsRequired();
        builder.Property(e => e.TipoEmailId).HasColumnName("tipo_email_id").IsRequired();
        builder.Property(e => e.Email).HasColumnName("email").HasMaxLength(150).IsRequired();
        builder.Property(e => e.EsPrincipal).HasColumnName("es_principal").IsRequired().HasDefaultValue(false);

        builder.HasIndex(e => e.Email).IsUnique();

        builder.HasOne(e => e.Aerolinea)
            .WithMany(a => a.Emails)
            .HasForeignKey(e => e.AerolineaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.TipoEmail)
            .WithMany(t => t.EmailsAerolinea)
            .HasForeignKey(e => e.TipoEmailId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}