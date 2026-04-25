// src/modules/bookingpassenger/Infrastructure/entity/BookingPassengerEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.bookingpassenger.Infrastructure.entity;

public class BookingPassengerEntityConfig : IEntityTypeConfiguration<BookingPassengerEntity>
{
    public void Configure(EntityTypeBuilder<BookingPassengerEntity> builder)
    {
        builder.ToTable("pasajeros_reserva");

        builder.HasKey(bp => bp.Id);
        builder.Property(bp => bp.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(bp => bp.ReservaId).HasColumnName("reserva_id").IsRequired();
        builder.Property(bp => bp.PersonaId).HasColumnName("persona_id").IsRequired();
        builder.Property(bp => bp.TipoPasajero).HasColumnName("tipo_pasajero")
            .HasMaxLength(10).IsRequired().HasDefaultValue("ADULTO");
        builder.Property(bp => bp.AsientoId).HasColumnName("asiento_id");

        builder.HasIndex(bp => new { bp.ReservaId, bp.PersonaId }).IsUnique();
        builder.ToTable(t => t.HasCheckConstraint("chk_tipo_pasajero",
            "tipo_pasajero IN ('ADULTO','MENOR','INFANTE')"));

        builder.HasOne(bp => bp.Reserva)
            .WithMany(b => b.Pasajeros)
            .HasForeignKey(bp => bp.ReservaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(bp => bp.Persona)
            .WithMany()
            .HasForeignKey(bp => bp.PersonaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(bp => bp.Asiento)
            .WithMany(sa => sa.PasajerosReserva)
            .HasForeignKey(bp => bp.AsientoId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}