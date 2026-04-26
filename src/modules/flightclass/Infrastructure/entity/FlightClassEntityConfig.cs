using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.flightclass.Infrastructure.entity;

public sealed class FlightClassEntityConfig : IEntityTypeConfiguration<FlightClassEntity>
{
    public void Configure(EntityTypeBuilder<FlightClassEntity> builder)
    {
        builder.ToTable("flight_classes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(10).IsRequired();
        builder.Property(x => x.Description).HasColumnName("description").HasMaxLength(200);

        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasIndex(x => x.Code).IsUnique();
    }
}

