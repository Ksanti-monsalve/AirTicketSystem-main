namespace AirTicketSystem.modules.flightclass.Infrastructure.entity;

public sealed class FlightClassEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;   // Economy, Business, FirstClass
    public string Code { get; set; } = null!;   // ECO, BUS, FST (o similar)
    public string? Description { get; set; }
}

