// src/modules/flight/Domain/aggregate/Flight.cs
using AirTicketSystem.modules.flight.Domain.ValueObjects;

namespace AirTicketSystem.modules.flight.Domain.aggregate;

public sealed class Flight
{
    public int Id { get; private set; }
    public int RutaId { get; private set; }
    public int AvionId { get; private set; }
    public int? PuertaEmbarqueId { get; private set; }
    public NumeroVueloFlight NumeroVuelo { get; private set; } = null!;
    public FechaSalidaFlight FechaSalida { get; private set; } = null!;
    public FechaLlegadaEstimadaFlight FechaLlegadaEstimada { get; private set; } = null!;
    public DateTime? FechaLlegadaReal { get; private set; }
    public DateTime? CheckinApertura { get; private set; }
    public DateTime? CheckinCierre { get; private set; }
    public EstadoFlight Estado { get; private set; } = null!;
    public MotivoCambioEstadoFlight? MotivoCambioEstado { get; private set; }

    private Flight() { }

    public static Flight Crear(
        int rutaId,
        int avionId,
        string numeroVuelo,
        DateTime fechaSalida,
        DateTime fechaLlegadaEstimada,
        int? puertaEmbarqueId = null)
    {
        if (rutaId <= 0)
            throw new ArgumentException("La ruta es obligatoria.");

        if (avionId <= 0)
            throw new ArgumentException("El avión es obligatorio.");

        var salidaVO  = FechaSalidaFlight.Crear(fechaSalida);
        var llegadaVO = FechaLlegadaEstimadaFlight.Crear(
            fechaLlegadaEstimada, fechaSalida);

        return new Flight
        {
            RutaId               = rutaId,
            AvionId              = avionId,
            PuertaEmbarqueId     = puertaEmbarqueId,
            NumeroVuelo          = NumeroVueloFlight.Crear(numeroVuelo),
            FechaSalida          = salidaVO,
            FechaLlegadaEstimada = llegadaVO,
            Estado               = EstadoFlight.Programado()
        };
    }

    public static Flight Reconstituir(
        int id,
        int rutaId,
        int avionId,
        int? puertaEmbarqueId,
        string numeroVuelo,
        DateTime fechaSalida,
        DateTime fechaLlegadaEstimada,
        DateTime? fechaLlegadaReal,
        DateTime? checkinApertura,
        DateTime? checkinCierre,
        string estado,
        string? motivoCambioEstado)
    {
        return new Flight
        {
            Id                   = id,
            RutaId               = rutaId,
            AvionId              = avionId,
            PuertaEmbarqueId     = puertaEmbarqueId,
            NumeroVuelo          = NumeroVueloFlight.Crear(numeroVuelo),
            FechaSalida          = FechaSalidaFlight.Reconstituir(fechaSalida),
            FechaLlegadaEstimada = FechaLlegadaEstimadaFlight
                .Reconstituir(fechaLlegadaEstimada),
            FechaLlegadaReal  = fechaLlegadaReal,
            CheckinApertura   = checkinApertura,
            CheckinCierre     = checkinCierre,
            Estado            = EstadoFlight.Crear(estado),
            MotivoCambioEstado = motivoCambioEstado is not null
                ? MotivoCambioEstadoFlight.Crear(motivoCambioEstado)
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

    // ── Operaciones de negocio ───────────────────────────────

    public void AbrirCheckin(DateTime apertura, DateTime cierre)
    {
        if (Estado.Valor != "PROGRAMADO" && Estado.Valor != "DEMORADO")
            throw new InvalidOperationException(
                $"No se puede abrir el check-in con el vuelo en estado '{Estado}'.");

        if (CheckinApertura.HasValue)
            throw new InvalidOperationException(
                "El check-in de este vuelo ya fue abierto.");

        CheckinAperturaFlight.Crear(apertura, FechaSalida.Valor);
        CheckinCierreFlight.Crear(cierre, FechaSalida.Valor, apertura);

        CheckinApertura = apertura;
        CheckinCierre   = cierre;
    }

    public void AsignarPuerta(int puertaEmbarqueId)
    {
        if (Estado.Valor == "CANCELADO" || Estado.Valor == "ATERRIZADO")
            throw new InvalidOperationException(
                $"No se puede asignar puerta a un vuelo en estado '{Estado}'.");

        PuertaEmbarqueId = puertaEmbarqueId;
    }

    public void IniciarAbordaje()
    {
        if (Estado.Valor != "PROGRAMADO" && Estado.Valor != "DEMORADO")
            throw new InvalidOperationException(
                $"No se puede iniciar abordaje con el vuelo en estado '{Estado}'.");

        if (!PuertaEmbarqueId.HasValue)
            throw new InvalidOperationException(
                "El vuelo no tiene puerta de embarque asignada.");

        Estado = EstadoFlight.Abordando();
        MotivoCambioEstado = null;
    }

    public void IniciarVuelo()
    {
        if (Estado.Valor != "ABORDANDO")
            throw new InvalidOperationException(
                $"Solo se puede iniciar un vuelo en estado ABORDANDO. " +
                $"Estado actual: '{Estado}'.");

        Estado = EstadoFlight.EnVuelo();
        MotivoCambioEstado = null;
    }

    public void RegistrarAterrizaje(DateTime fechaLlegadaReal)
    {
        if (Estado.Valor != "EN_VUELO" && Estado.Valor != "DESVIADO")
            throw new InvalidOperationException(
                $"Solo se puede registrar aterrizaje de vuelos EN_VUELO o DESVIADOS. " +
                $"Estado actual: '{Estado}'.");

        var llegadaVO = FechaLlegadaRealFlight.Crear(
            fechaLlegadaReal, FechaSalida.Valor);

        FechaLlegadaReal   = llegadaVO.Valor;
        Estado             = EstadoFlight.Aterrizado();
        MotivoCambioEstado = null;
    }

    public void Cancelar(string motivo)
    {
        if (Estado.Valor == "EN_VUELO")
            throw new InvalidOperationException(
                "No se puede cancelar un vuelo que ya está en el aire.");

        if (Estado.Valor == "ATERRIZADO")
            throw new InvalidOperationException(
                "No se puede cancelar un vuelo que ya aterrizó.");

        if (Estado.Valor == "CANCELADO")
            throw new InvalidOperationException("El vuelo ya está cancelado.");

        MotivoCambioEstado = MotivoCambioEstadoFlight.Crear(motivo);
        Estado             = EstadoFlight.Cancelado();
    }

    public void Demorar(string motivo)
    {
        if (Estado.Valor != "PROGRAMADO")
            throw new InvalidOperationException(
                $"Solo se pueden demorar vuelos PROGRAMADOS. " +
                $"Estado actual: '{Estado}'.");

        MotivoCambioEstado = MotivoCambioEstadoFlight.Crear(motivo);
        Estado             = EstadoFlight.Demorado();
    }

    public void Desviar(string motivo)
    {
        if (Estado.Valor != "EN_VUELO")
            throw new InvalidOperationException(
                $"Solo se pueden desviar vuelos EN_VUELO. " +
                $"Estado actual: '{Estado}'.");

        MotivoCambioEstado = MotivoCambioEstadoFlight.Crear(motivo);
        Estado             = EstadoFlight.Desviado();
    }

    public void ActualizarHorarios(
        DateTime fechaSalida, DateTime fechaLlegadaEstimada)
    {
        if (Estado.Valor != "PROGRAMADO" && Estado.Valor != "DEMORADO")
            throw new InvalidOperationException(
                $"Solo se pueden modificar horarios de vuelos PROGRAMADOS o DEMORADOS. " +
                $"Estado actual: '{Estado}'.");

        FechaSalida = FechaSalidaFlight.Crear(fechaSalida);
        FechaLlegadaEstimada = FechaLlegadaEstimadaFlight.Crear(
            fechaLlegadaEstimada, fechaSalida);
    }

    public bool CheckinEstaAbierto
    {
        get
        {
            if (!CheckinApertura.HasValue || !CheckinCierre.HasValue)
                return false;

            var ahora = DateTime.UtcNow;
            return ahora >= CheckinApertura.Value && ahora <= CheckinCierre.Value;
        }
    }

    public decimal DuracionEstimadaHoras
        => (decimal)(FechaLlegadaEstimada.Valor - FechaSalida.Valor).TotalHours;

    public override string ToString()
        => $"[{NumeroVuelo}] {FechaSalida} | {Estado}";
}