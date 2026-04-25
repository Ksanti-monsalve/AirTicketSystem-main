// src/modules/checkin/Domain/aggregate/CheckIn.cs
using AirTicketSystem.modules.checkin.Domain.ValueObjects;

namespace AirTicketSystem.modules.checkin.Domain.aggregate;

public sealed class CheckIn
{
    public int Id { get; private set; }
    public int PasajeroReservaId { get; private set; }
    public int? TrabajadorId { get; private set; }
    public TipoCheckin Tipo { get; private set; } = null!;
    public FechaCheckinCheckin FechaCheckin { get; private set; } = null!;
    public EstadoCheckin Estado { get; private set; } = null!;

    private CheckIn() { }

    // ── Creación por tipo ────────────────────────────────────

    public static CheckIn CrearVirtual(int pasajeroReservaId)
    {
        if (pasajeroReservaId <= 0)
            throw new ArgumentException(
                "El pasajero de reserva es obligatorio.");

        return new CheckIn
        {
            PasajeroReservaId = pasajeroReservaId,
            TrabajadorId      = null,
            Tipo              = TipoCheckin.Virtual(),
            FechaCheckin      = FechaCheckinCheckin.Ahora(),
            Estado            = EstadoCheckin.Pendiente()
        };
    }

    public static CheckIn CrearPresencial(
        int pasajeroReservaId,
        int trabajadorId)
    {
        if (pasajeroReservaId <= 0)
            throw new ArgumentException(
                "El pasajero de reserva es obligatorio.");

        if (trabajadorId <= 0)
            throw new ArgumentException(
                "El trabajador es obligatorio para el check-in presencial.");

        return new CheckIn
        {
            PasajeroReservaId = pasajeroReservaId,
            TrabajadorId      = trabajadorId,
            Tipo              = TipoCheckin.Presencial(),
            FechaCheckin      = FechaCheckinCheckin.Ahora(),
            Estado            = EstadoCheckin.Pendiente()
        };
    }

    // ── Máquina de estados ───────────────────────────────────

    public void Completar()
    {
        if (!Estado.PuedeTransicionarA(EstadoCheckin.Completado()))
            throw new InvalidOperationException(
                $"No se puede completar el check-in en estado '{Estado}'.");

        // El check-in presencial requiere trabajador asignado
        if (Tipo.RequiereTrabajador && TrabajadorId is null)
            throw new InvalidOperationException(
                "El check-in presencial requiere un trabajador asignado " +
                "antes de completarse.");

        Estado = EstadoCheckin.Completado();
    }

    public void Cancelar()
    {
        if (!Estado.PuedeTransicionarA(EstadoCheckin.Cancelado()))
            throw new InvalidOperationException(
                $"No se puede cancelar el check-in en estado '{Estado}'. " +
                "Un check-in completado no puede cancelarse.");

        Estado = EstadoCheckin.Cancelado();
    }

    // ── Gestión de trabajador ────────────────────────────────

    public void AsignarTrabajador(int trabajadorId)
    {
        if (trabajadorId <= 0)
            throw new ArgumentException("El trabajador no es válido.");

        if (Tipo.EsVirtual)
            throw new InvalidOperationException(
                "No se puede asignar trabajador a un check-in virtual.");

        if (Estado.EstaCompletado)
            throw new InvalidOperationException(
                "No se puede cambiar el trabajador de un check-in completado.");

        TrabajadorId = trabajadorId;
    }

    public void RemoverTrabajador()
    {
        if (Tipo.EsPresencial)
            throw new InvalidOperationException(
                "No se puede remover el trabajador de un check-in presencial. " +
                "El check-in presencial siempre requiere un trabajador.");

        TrabajadorId = null;
    }

    // ── Validación de ventana de check-in ────────────────────

    /// <summary>
    /// Verifica que el check-in se realiza dentro de la ventana
    /// válida definida por el vuelo.
    /// </summary>
    public bool EstaEnVentanaValida(DateTime apertura, DateTime cierre)
    {
        return FechaCheckin.EstaEnVentanaValida(apertura, cierre);
    }

    /// <summary>
    /// Valida que el check-in no se realice demasiado tarde
    /// según el tipo de check-in y la hora del vuelo.
    /// </summary>
    public bool PuedeRealizarseAntesDeVuelo(DateTime fechaSalidaVuelo)
    {
        var minutosLimite = Tipo.MinutosLimiteAntesDeVuelo;
        var limiteCheckin = fechaSalidaVuelo.AddMinutes(-minutosLimite);
        return FechaCheckin.Valor <= limiteCheckin;
    }

    // ── Propiedades de negocio ───────────────────────────────

    public bool EstaCompletado => Estado.EstaCompletado;

    public bool EstaPendiente => Estado.EstaPendiente;

    public bool EstaCancelado => Estado.EstaCancelado;

    public bool PermiteGenerarPaseAbordar =>
        Estado.PermiteGenerarPaseAbordar;

    public bool EsVirtual => Tipo.EsVirtual;

    public bool EsPresencial => Tipo.EsPresencial;

    public bool TieneTrabajadorAsignado => TrabajadorId.HasValue;

    public static CheckIn Reconstituir(
        int id,
        int pasajeroReservaId,
        int? trabajadorId,
        string tipo,
        DateTime fechaCheckin,
        string estado)
    {
        var checkIn = new CheckIn
        {
            PasajeroReservaId = pasajeroReservaId,
            TrabajadorId      = trabajadorId,
            Tipo              = TipoCheckin.Crear(tipo),
            FechaCheckin      = FechaCheckinCheckin.Crear(fechaCheckin),
            Estado            = EstadoCheckin.Crear(estado)
        };
        checkIn.Id = id;
        return checkIn;
    }

    public void EstablecerId(int id) => Id = id;

    public override string ToString() =>
        $"Check-in [{Tipo}] — {Estado} | " +
        $"Pasajero #{PasajeroReservaId} | " +
        $"{FechaCheckin.EnFormatoCorto}";
}