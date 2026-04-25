// src/modules/phonetype/Infrastructure/entity/PhoneTypeEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.phonetype.Infrastructure.entity;

public class PhoneTypeEntityConfig : IEntityTypeConfiguration<PhoneTypeEntity>
{
    public void Configure(EntityTypeBuilder<PhoneTypeEntity> builder)
    {
        builder.ToTable("tipos_telefono");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(p => p.Descripcion).HasColumnName("descripcion").HasMaxLength(50).IsRequired();
        builder.HasIndex(p => p.Descripcion).IsUnique();
    }
}