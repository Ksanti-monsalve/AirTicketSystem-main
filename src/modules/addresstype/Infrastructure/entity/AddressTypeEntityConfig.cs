// src/modules/addresstype/Infrastructure/entity/AddressTypeEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.addresstype.Infrastructure.entity;

public class AddressTypeEntityConfig : IEntityTypeConfiguration<AddressTypeEntity>
{
    public void Configure(EntityTypeBuilder<AddressTypeEntity> builder)
    {
        builder.ToTable("tipos_direccion");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(a => a.Descripcion).HasColumnName("descripcion").HasMaxLength(50).IsRequired();
        builder.HasIndex(a => a.Descripcion).IsUnique();
    }
}