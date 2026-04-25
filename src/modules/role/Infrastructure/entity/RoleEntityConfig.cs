// src/modules/role/Infrastructure/entity/RoleEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.role.Infrastructure.entity;

public class RoleEntityConfig : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.ToTable("roles");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(r => r.Nombre).HasColumnName("nombre").HasMaxLength(50).IsRequired();
        builder.HasIndex(r => r.Nombre).IsUnique();
    }
}