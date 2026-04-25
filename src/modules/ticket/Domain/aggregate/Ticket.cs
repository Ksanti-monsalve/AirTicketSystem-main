// src/modules/ticket/Domain/aggregate/Ticket.cs
using AirTicketSystem.modules.ticket.Domain.ValueObjects;

namespace AirTicketSystem.modules.ticket.Domain.aggregate;

public sealed class Ticket
{
    public int Id { get; private set; }
    public int PasajeroReservaId { get; private set; }
    public int? AsientoConfirmadoId { get; private set; }
    public CodigoTiqueteTicket CodigoTiquete { get; private set; } = null!;
    public FechaEmisionTicket FechaEmision { get; private set; } = null!;
    public EstadoTicket Estado { get; private set; } = null!;

    private Ticket() { }

    public static Ticket Crear(
        int pasajeroReservaId,
        int? asientoConfirmadoId = null)
    {
        if (pasajeroReservaId <= 0)
            throw new ArgumentException(
                "El pasajero de reserva es obligatorio.");

        return new Ticket
        {
            PasajeroReservaId   = pasajeroReservaId,
            AsientoConfirmadoId = asientoConfirmadoId,
            CodigoTiquete       = CodigoTiqueteTicket.Generar(),
            FechaEmision        = FechaEmisionTicket.Ahora(),
            Estado              = EstadoTicket.Emitido()
        };
    }

    // ── Máquina de estados ───────────────────────────────────

    public void RegistrarCheckin(int? asientoConfirmadoId = null)
    {
        if (!Estado.PermiteCheckin)
            throw new InvalidOperationException(
                $"No se puede hacer check-in con el tiquete en estado '{Estado}'.");

        // El asiento puede confirmarse o reasignarse en el check-in
        if (asientoConfirmadoId.HasValue)
        {
            if (asientoConfirmadoId <= 0)
                throw new ArgumentException(
                    "El asiento confirmado no es válido.");

            AsientoConfirmadoId = asientoConfirmadoId;
        }

        CambiarEstado(EstadoTicket.CheckinHecho());
    }

    public void RegistrarAbordaje()
    {
        if (!Estado.PermiteAbordaje)
            throw new InvalidOperationException(
                $"No se puede registrar abordaje con el tiquete en estado '{Estado}'.");

        CambiarEstado(EstadoTicket.Abordado());
    }

    public void MarcarComoUsado()
    {
        if (!Estado.PuedeTransicionarA(EstadoTicket.Usado()))
            throw new InvalidOperationException(
                $"No se puede marcar como usado el tiquete en estado '{Estado}'.");

        CambiarEstado(EstadoTicket.Usado());
    }

    public void Anular()
    {
        if (!Estado.PuedeAnularse)
            throw new InvalidOperationException(
                $"No se puede anular el tiquete en estado '{Estado}'. " +
                "Solo pueden anularse tiquetes emitidos o con check-in hecho.");

        CambiarEstado(EstadoTicket.Anulado());
    }

    private void CambiarEstado(EstadoTicket nuevoEstado)
    {
        if (!Estado.PuedeTransicionarA(nuevoEstado))
            throw new InvalidOperationException(
                $"Transición de estado inválida: '{Estado}' → '{nuevoEstado}'.");

        Estado = nuevoEstado;
    }

    // ── Gestión de asiento ───────────────────────────────────

    public void ReasignarAsiento(int nuevoAsientoId)
    {
        if (nuevoAsientoId <= 0)
            throw new ArgumentException("El nuevo asiento no es válido.");

        if (Estado.EstaFinalizado)
            throw new InvalidOperationException(
                "No se puede reasignar asiento a un tiquete finalizado.");

        if (nuevoAsientoId == AsientoConfirmadoId)
            throw new InvalidOperationException(
                "El nuevo asiento es el mismo que el asiento actual.");

        AsientoConfirmadoId = nuevoAsientoId;
    }

    // ── Propiedades de negocio ───────────────────────────────

    public bool EstaVigente => Estado.EstaVigente;

    public bool EstaFinalizado => Estado.EstaFinalizado;

    public bool PuedeAnularse => Estado.PuedeAnularse;

    public bool TieneAsientoConfirmado => AsientoConfirmadoId.HasValue;

    public bool CheckinRealizado =>
        Estado.Valor == "CHECKIN_HECHO" ||
        Estado.Valor == "ABORDADO"      ||
        Estado.Valor == "USADO";

    public bool FueAbordado =>
        Estado.Valor == "ABORDADO" || Estado.Valor == "USADO";

    public static Ticket Reconstituir(
        int id,
        int pasajeroReservaId,
        int? asientoConfirmadoId,
        string codigoTiquete,
        DateTime fechaEmision,
        string estado)
    {
        var ticket = new Ticket
        {
            PasajeroReservaId   = pasajeroReservaId,
            AsientoConfirmadoId = asientoConfirmadoId,
            CodigoTiquete       = CodigoTiqueteTicket.Crear(codigoTiquete),
            FechaEmision        = FechaEmisionTicket.Crear(fechaEmision),
            Estado              = EstadoTicket.Crear(estado)
        };
        ticket.Id = id;
        return ticket;
    }

    public void EstablecerId(int id) => Id = id;

    public override string ToString() =>
        $"[{CodigoTiquete}] — {Estado} | " +
        $"Emitido: {FechaEmision.EnFormatoCorto}";
}