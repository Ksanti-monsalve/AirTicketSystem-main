// src/modules/flighthistory/Domain/aggregate/FlightHistory.cs
using AirTicketSystem.modules.flighthistory.Domain.ValueObjects;

namespace AirTicketSystem.modules.flighthistory.Domain.aggregate;

public sealed class FlightHistory
{
    public int Id { get; private set; }
    public int VueloId { get; private set; }
    public int? UsuarioId { get; private set; }
    public EstadoAnteriorFlightHistory EstadoAnterior { get; private set; } = null!;
    public EstadoNuevoFlightHistory EstadoNuevo { get; private set; } = null!;
    public DateTime FechaCambio { get; private set; }
    public MotivoFlightHistory? Motivo { get; private set; }

    private FlightHistory() { }

    public static FlightHistory Crear(
        int vueloId,
        string estadoAnterior,
        string estadoNuevo,
        int? usuarioId = null,
        string? motivo = null)
    {
        if (vueloId <= 0)
            throw new ArgumentException("El vuelo es obligatorio.");

        if (estadoAnterior == estadoNuevo)
            throw new InvalidOperationException(
                "El estado anterior y el nuevo no pueden ser iguales.");

        return new FlightHistory
        {
            VueloId        = vueloId,
            UsuarioId      = usuarioId,
            EstadoAnterior = EstadoAnteriorFlightHistory.Crear(estadoAnterior),
            EstadoNuevo    = EstadoNuevoFlightHistory.Crear(estadoNuevo),
            FechaCambio    = DateTime.UtcNow,
            Motivo         = motivo is not null
                ? MotivoFlightHistory.Crear(motivo)
                : null
        };
    }

    public static FlightHistory Reconstituir(
        int id,
        int vueloId,
        int? usuarioId,
        string estadoAnterior,
        string estadoNuevo,
        DateTime fechaCambio,
        string? motivo)
    {
        return new FlightHistory
        {
            Id             = id,
            VueloId        = vueloId,
            UsuarioId      = usuarioId,
            EstadoAnterior = EstadoAnteriorFlightHistory.Crear(estadoAnterior),
            EstadoNuevo    = EstadoNuevoFlightHistory.Crear(estadoNuevo),
            FechaCambio    = fechaCambio,
            Motivo         = motivo is not null
                ? MotivoFlightHistory.Crear(motivo)
                : null
        };
    }

    public void EstablecerId(int id)
    {
        if (Id != 0)
            throw new InvalidOperationException("El ID ya fue establecido.");

        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        Id = id;
    }

    public override string ToString()
        => $"[{FechaCambio:dd/MM/yyyy HH:mm}] {EstadoAnterior} → {EstadoNuevo}";
}