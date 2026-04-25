// src/modules/country/Infrastructure/entity/CountryEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.country.Infrastructure.entity;

public class CountryEntityConfig : IEntityTypeConfiguration<CountryEntity>
{
    public void Configure(EntityTypeBuilder<CountryEntity> builder)
    {
        builder.ToTable("paises");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(c => c.ContinenteId).HasColumnName("continente_id").IsRequired();
        builder.Property(c => c.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
        builder.Property(c => c.CodigoIso2).HasColumnName("codigo_iso2").HasMaxLength(2).IsRequired().IsFixedLength();
        builder.Property(c => c.CodigoIso3).HasColumnName("codigo_iso3").HasMaxLength(3).IsRequired().IsFixedLength();

        builder.HasIndex(c => c.Nombre).IsUnique();
        builder.HasIndex(c => c.CodigoIso2).IsUnique();
        builder.HasIndex(c => c.CodigoIso3).IsUnique();

        builder.HasOne(c => c.Continente)
            .WithMany(co => co.Paises)
            .HasForeignKey(c => c.ContinenteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
