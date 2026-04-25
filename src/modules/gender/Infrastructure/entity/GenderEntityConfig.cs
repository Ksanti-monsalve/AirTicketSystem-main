// src/modules/gender/Infrastructure/entity/GenderEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.gender.Infrastructure.entity;

public class GenderEntityConfig : IEntityTypeConfiguration<GenderEntity>
{
    public void Configure(EntityTypeBuilder<GenderEntity> builder)
    {
        builder.ToTable("generos");
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(g => g.Nombre).HasColumnName("nombre").HasMaxLength(50).IsRequired();
        builder.HasIndex(g => g.Nombre).IsUnique();
    }
}