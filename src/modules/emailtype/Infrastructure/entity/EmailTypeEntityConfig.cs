// src/modules/emailtype/Infrastructure/entity/EmailTypeEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.emailtype.Infrastructure.entity;

public class EmailTypeEntityConfig : IEntityTypeConfiguration<EmailTypeEntity>
{
    public void Configure(EntityTypeBuilder<EmailTypeEntity> builder)
    {
        builder.ToTable("tipos_email");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(50).IsRequired();
        builder.HasIndex(e => e.Descripcion).IsUnique();
    }
}