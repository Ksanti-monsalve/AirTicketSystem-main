// src/modules/seatavailability/Domain/aggregate/SeatAvailability.cs
using AirTicketSystem.modules.seatavailability.Domain.ValueObjects;

namespace AirTicketSystem.modules.seatavailability.Domain.aggregate;

public sealed class SeatAvailability
{
    public int Id { get; private set; }
    public int VueloId { get; private set; }
    public int AsientoId { get; private set; }
    public EstadoSeatAvailability Estado { get; private set; } = null!;

    private SeatAvailability() { }

    public static SeatAvailability Crear(int vueloId, int asientoId)
    {
        if (vueloId <= 0)
            throw new ArgumentException("El vuelo es obligatorio.");

        if (asientoId <= 0)
            throw new ArgumentException("El asiento es obligatorio.");

        return new SeatAvailability
        {
            VueloId   = vueloId,
            AsientoId = asientoId,
            Estado    = EstadoSeatAvailability.Disponible()
        };
    }

    public static SeatAvailability Reconstituir(
        int id, int vueloId, int asientoId, string estado)
    {
        return new SeatAvailability
        {
            Id        = id,
            VueloId   = vueloId,
            AsientoId = asientoId,
            Estado    = EstadoSeatAvailability.Crear(estado)
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

    public void Reservar()
    {
        if (Estado.Valor != "DISPONIBLE")
            throw new InvalidOperationException(
                $"No se puede reservar un asiento en estado '{Estado}'.");

        Estado = EstadoSeatAvailability.Reservado();
    }

    public void Liberar()
    {
        if (Estado.Valor != "RESERVADO")
            throw new InvalidOperationException(
                $"Solo se pueden liberar asientos RESERVADOS. " +
                $"Estado actual: '{Estado}'.");

        Estado = EstadoSeatAvailability.Disponible();
    }

    public void Bloquear()
    {
        if (Estado.Valor != "DISPONIBLE")
            throw new InvalidOperationException(
                $"Solo se pueden bloquear asientos DISPONIBLES. " +
                $"Estado actual: '{Estado}'.");

        Estado = EstadoSeatAvailability.Bloqueado();
    }

    public void Desbloquear()
    {
        if (Estado.Valor != "BLOQUEADO")
            throw new InvalidOperationException(
                $"Solo se pueden desbloquear asientos BLOQUEADOS. " +
                $"Estado actual: '{Estado}'.");

        Estado = EstadoSeatAvailability.Disponible();
    }

    public bool EstaDisponible => Estado.Valor == "DISPONIBLE";

    public override string ToString()
        => $"Asiento #{AsientoId} — Vuelo #{VueloId} | {Estado}";
}