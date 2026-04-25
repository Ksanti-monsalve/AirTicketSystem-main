// src/modules/city/Infrastructure/entity/CityEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.city.Infrastructure.entity;

public class CityEntityConfig : IEntityTypeConfiguration<CityEntity>
{
    public void Configure(EntityTypeBuilder<CityEntity> builder)
    {
        builder.ToTable("ciudades");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(c => c.DepartamentoId).HasColumnName("departamento_id").IsRequired();
        builder.Property(c => c.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
        builder.Property(c => c.CodigoPostal).HasColumnName("codigo_postal").HasMaxLength(10);

        builder.HasIndex(c => new { c.DepartamentoId, c.Nombre }).IsUnique();

        builder.HasOne(c => c.Departamento)
            .WithMany(d => d.Ciudades)
            .HasForeignKey(c => c.DepartamentoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}