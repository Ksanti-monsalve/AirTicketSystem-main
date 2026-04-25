// src/modules/luggagetype/Infrastructure/entity/LuggageTypeEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.luggagetype.Infrastructure.entity;

public class LuggageTypeEntityConfig : IEntityTypeConfiguration<LuggageTypeEntity>
{
    public void Configure(EntityTypeBuilder<LuggageTypeEntity> builder)
    {
        builder.ToTable("tipos_equipaje");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(l => l.Nombre).HasColumnName("nombre").HasMaxLength(50).IsRequired();
        builder.HasIndex(l => l.Nombre).IsUnique();
    }
}