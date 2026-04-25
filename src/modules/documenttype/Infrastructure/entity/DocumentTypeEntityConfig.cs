// src/modules/documenttype/Infrastructure/entity/DocumentTypeEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.documenttype.Infrastructure.entity;

public class DocumentTypeEntityConfig : IEntityTypeConfiguration<DocumentTypeEntity>
{
    public void Configure(EntityTypeBuilder<DocumentTypeEntity> builder)
    {
        builder.ToTable("tipos_documento");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(d => d.Descripcion).HasColumnName("descripcion").HasMaxLength(50).IsRequired();
        builder.HasIndex(d => d.Descripcion).IsUnique();
    }
}