using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AirTicketSystem.modules.booking.Infrastructure.entity;
using AirTicketSystem.modules.flight.Infrastructure.entity;
using AirTicketSystem.modules.flightclass.Infrastructure.entity;
using AirTicketSystem.modules.ticket.Infrastructure.entity;

namespace AirTicketSystem.modules.seat.Infrastructure.entity;

public sealed class SeatEntityConfig : IEntityTypeConfiguration<SeatEntity>
{
    public void Configure(EntityTypeBuilder<SeatEntity> builder)
    {
        builder.ToTable("seats");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(s => s.FlightId).HasColumnName("flight_id").IsRequired();
        builder.Property(s => s.SeatNumber).HasColumnName("seat_number").HasMaxLength(5).IsRequired();
        builder.Property(s => s.FlightClassId).HasColumnName("flight_class_id").IsRequired();
        builder.Property(s => s.Status).HasColumnName("status").HasMaxLength(15).IsRequired()
            .HasDefaultValue("Available");
        builder.Property(s => s.BookingId).HasColumnName("booking_id");
        builder.Property(s => s.TicketId).HasColumnName("ticket_id");

        builder.HasIndex(s => new { s.FlightId, s.SeatNumber }).IsUnique();
        builder.HasIndex(s => s.BookingId);
        builder.HasIndex(s => s.TicketId);
        builder.ToTable(t => t.HasCheckConstraint("chk_seat_status",
            "status IN ('Available','Reserved','Occupied','Blocked')"));

        builder.HasOne(s => s.Flight)
            .WithMany()
            .HasForeignKey(s => s.FlightId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.FlightClass)
            .WithMany()
            .HasForeignKey(s => s.FlightClassId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Booking)
            .WithMany()
            .HasForeignKey(s => s.BookingId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(s => s.Ticket)
            .WithMany()
            .HasForeignKey(s => s.TicketId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

