// src/modules/bookingpassenger/Domain/aggregate/BookingPassenger.cs
using AirTicketSystem.modules.bookingpassenger.Domain.ValueObjects;

namespace AirTicketSystem.modules.bookingpassenger.Domain.aggregate;

public sealed class BookingPassenger
{
    public int Id { get; private set; }
    public int ReservaId { get; private set; }
    public int PersonaId { get; private set; }
    public int? AsientoId { get; private set; }
    public TipoPasajeroBookingPassenger TipoPasajero { get; private set; } = null!;

    private BookingPassenger() { }

    public static BookingPassenger Crear(
        int reservaId,
        int personaId,
        string tipoPasajero,
        int? asientoId = null)
    {
        if (reservaId <= 0)
            throw new ArgumentException("La reserva es obligatoria.");

        if (personaId <= 0)
            throw new ArgumentException("La persona es obligatoria.");

        return new BookingPassenger
        {
            ReservaId   = reservaId,
            PersonaId   = personaId,
            TipoPasajero = TipoPasajeroBookingPassenger.Crear(tipoPasajero),
            AsientoId   = asientoId
        };
    }

    // Métodos de fábrica expresivos por tipo
    public static BookingPassenger CrearAdulto(
        int reservaId,
        int personaId,
        int? asientoId = null)
        => Crear(reservaId, personaId, "ADULTO", asientoId);

    public static BookingPassenger CrearMenor(
        int reservaId,
        int personaId,
        int? asientoId = null)
        => Crear(reservaId, personaId, "MENOR", asientoId);

    public static BookingPassenger CrearInfante(
        int reservaId,
        int personaId)
        => Crear(reservaId, personaId, "INFANTE", null);

    // ── Gestión de asiento ───────────────────────────────────

    public void AsignarAsiento(int asientoId)
    {
        if (asientoId <= 0)
            throw new ArgumentException("El asiento no es válido.");

        if (TipoPasajero.ViajaEnFalda)
            throw new InvalidOperationException(
                "Un infante no puede tener asiento propio. Viaja en la falda de un adulto.");

        if (AsientoId.HasValue)
            throw new InvalidOperationException(
                "El pasajero ya tiene un asiento asignado. " +
                "Libere el asiento actual antes de asignar uno nuevo.");

        AsientoId = asientoId;
    }

    public void LiberarAsiento()
    {
        if (!AsientoId.HasValue)
            throw new InvalidOperationException(
                "El pasajero no tiene asiento asignado.");

        AsientoId = null;
    }

    public void CambiarAsiento(int nuevoAsientoId)
    {
        if (nuevoAsientoId <= 0)
            throw new ArgumentException("El nuevo asiento no es válido.");

        if (TipoPasajero.ViajaEnFalda)
            throw new InvalidOperationException(
                "Un infante no puede tener asiento propio.");

        if (nuevoAsientoId == AsientoId)
            throw new InvalidOperationException(
                "El nuevo asiento es el mismo que el actual.");

        AsientoId = nuevoAsientoId;
    }

    // ── Propiedades de negocio ───────────────────────────────

    public bool TieneAsientoAsignado => AsientoId.HasValue;

    public bool EsAdulto => TipoPasajero.Valor == "ADULTO";

    public bool EsMenor => TipoPasajero.Valor == "MENOR";

    public bool EsInfante => TipoPasajero.ViajaEnFalda;

    public bool RequiereAsientoPropio =>
        TipoPasajero.RequiereAsientoPropio;

    public bool RequiereAcompanante =>
        TipoPasajero.RequiereAdultoAcompanante;

    public static BookingPassenger Reconstituir(
        int id,
        int reservaId,
        int personaId,
        string tipoPasajero,
        int? asientoId)
    {
        var passenger = Crear(reservaId, personaId, tipoPasajero, asientoId);
        passenger.Id = id;
        return passenger;
    }

    public void EstablecerId(int id) => Id = id;

    public override string ToString() =>
        $"Pasajero [{TipoPasajero}] — Persona #{PersonaId} " +
        $"| Asiento: {(AsientoId.HasValue ? AsientoId.ToString() : "Sin asignar")}";
}