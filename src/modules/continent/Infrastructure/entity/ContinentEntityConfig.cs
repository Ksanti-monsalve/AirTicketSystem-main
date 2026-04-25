// src/modules/continent/Infrastructure/entity/ContinentEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.continent.Infrastructure.entity;

public class ContinentEntityConfig : IEntityTypeConfiguration<ContinentEntity>
{
    public void Configure(EntityTypeBuilder<ContinentEntity> builder)
    {
        builder.ToTable("continentes");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(c => c.Nombre)
            .HasColumnName("nombre")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Codigo)
            .HasColumnName("codigo")
            .HasMaxLength(2)
            .IsRequired()
            .IsFixedLength();

        builder.HasIndex(c => c.Nombre).IsUnique();
        builder.HasIndex(c => c.Codigo).IsUnique();
    }
}
