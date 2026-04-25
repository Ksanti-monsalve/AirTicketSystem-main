// src/modules/boardingpass/Domain/aggregate/BoardingPass.cs
using AirTicketSystem.modules.boardingpass.Domain.ValueObjects;

namespace AirTicketSystem.modules.boardingpass.Domain.aggregate;

public sealed class BoardingPass
{
    public int Id { get; private set; }
    public int CheckinId { get; private set; }
    public int? PuertaEmbarqueId { get; private set; }
    public CodigoPaseBoardingPass CodigoPase { get; private set; } = null!;
    public CodigoQrBoardingPass CodigoQr { get; private set; } = null!;
    public HoraEmbarcBoardingPass? HoraEmbarque { get; private set; }
    public FechaEmisionBoardingPass FechaEmision { get; private set; } = null!;

    private BoardingPass() { }

    public static BoardingPass Crear(
        int checkinId,
        string numeroVuelo,
        string codigoAsiento,
        DateTime fechaSalidaVuelo,
        int? puertaEmbarqueId = null,
        DateTime? horaEmbarque = null)
    {
        if (checkinId <= 0)
            throw new ArgumentException("El check-in es obligatorio.");

        if (string.IsNullOrWhiteSpace(numeroVuelo))
            throw new ArgumentException("El número de vuelo es obligatorio.");

        if (string.IsNullOrWhiteSpace(codigoAsiento))
            throw new ArgumentException("El código de asiento es obligatorio.");

        var codigoPase = CodigoPaseBoardingPass.Generar();

        var codigoQr = CodigoQrBoardingPass.Generar(
            codigoPase.Valor,
            numeroVuelo,
            codigoAsiento,
            fechaSalidaVuelo);

        return new BoardingPass
        {
            CheckinId        = checkinId,
            PuertaEmbarqueId = puertaEmbarqueId,
            CodigoPase       = codigoPase,
            CodigoQr         = codigoQr,
            HoraEmbarque     = horaEmbarque is not null
                ? HoraEmbarcBoardingPass.Crear(
                    horaEmbarque.Value,
                    fechaSalidaVuelo)
                : null,
            FechaEmision = FechaEmisionBoardingPass.Ahora()
        };
    }

    // ── Gestión de puerta y embarque ─────────────────────────

    public void AsignarPuerta(int puertaEmbarqueId)
    {
        if (puertaEmbarqueId <= 0)
            throw new ArgumentException(
                "La puerta de embarque no es válida.");

        if (puertaEmbarqueId == PuertaEmbarqueId)
            throw new InvalidOperationException(
                "La puerta indicada es la misma que la puerta actual.");

        PuertaEmbarqueId = puertaEmbarqueId;
    }

    public void LiberarPuerta()
    {
        if (PuertaEmbarqueId is null)
            throw new InvalidOperationException(
                "El pase de abordar no tiene puerta asignada.");

        PuertaEmbarqueId = null;
    }

    public void AsignarHoraEmbarque(
        DateTime horaEmbarque,
        DateTime fechaSalidaVuelo)
    {
        if (HoraEmbarque is not null)
            throw new InvalidOperationException(
                "El pase ya tiene hora de embarque asignada. " +
                "Use ActualizarHoraEmbarque para modificarla.");

        HoraEmbarque = HoraEmbarcBoardingPass.Crear(
            horaEmbarque, fechaSalidaVuelo);
    }

    public void ActualizarHoraEmbarque(
        DateTime nuevaHoraEmbarque,
        DateTime fechaSalidaVuelo)
    {
        if (HoraEmbarque is null)
            throw new InvalidOperationException(
                "No hay hora de embarque asignada. " +
                "Use AsignarHoraEmbarque para asignar una.");

        var nuevaHora = HoraEmbarcBoardingPass.Crear(
            nuevaHoraEmbarque, fechaSalidaVuelo);

        if (nuevaHora.Valor == HoraEmbarque.Valor)
            throw new InvalidOperationException(
                "La nueva hora de embarque es igual a la actual.");

        HoraEmbarque = nuevaHora;
    }

    // ── Propiedades de negocio ───────────────────────────────

    public bool TienePuertaAsignada =>
        PuertaEmbarqueId.HasValue;

    public bool TieneHoraEmbarque =>
        HoraEmbarque is not null;

    public bool EmbarqueYaComenzo =>
        HoraEmbarque is not null && HoraEmbarque.YaComenzo;

    public int? MinutosParaEmbarque =>
        HoraEmbarque?.MinutosRestantes;

    public bool EstaCompleto =>
        TienePuertaAsignada && TieneHoraEmbarque;

    public static BoardingPass Reconstituir(
        int id,
        int checkinId,
        string codigoPase,
        string? codigoQr,
        int? puertaEmbarqueId,
        DateTime? horaEmbarque,
        DateTime fechaEmision)
    {
        var bp = new BoardingPass
        {
            CheckinId        = checkinId,
            PuertaEmbarqueId = puertaEmbarqueId,
            CodigoPase       = CodigoPaseBoardingPass.Crear(codigoPase),
            CodigoQr         = codigoQr is not null
                ? CodigoQrBoardingPass.Crear(codigoQr)
                : throw new InvalidOperationException(
                    "El código QR del pase de abordar no puede ser nulo."),
            HoraEmbarque     = horaEmbarque.HasValue
                ? HoraEmbarcBoardingPass.Reconstituir(horaEmbarque.Value)
                : null,
            FechaEmision     = FechaEmisionBoardingPass.Crear(fechaEmision)
        };
        bp.Id = id;
        return bp;
    }

    public void EstablecerId(int id) => Id = id;

    /// <summary>
    /// Genera un resumen del pase para mostrar en consola.
    /// Incluye toda la información que el pasajero necesita.
    /// </summary>
    public string ResumenParaPasajero
    {
        get
        {
            var lineas = new List<string>
            {
                $"=== PASE DE ABORDAR ===",
                $"Código    : {CodigoPase}",
                $"Emitido   : {FechaEmision.EnFormatoCorto}",
                $"Puerta    : {(TienePuertaAsignada ? $"#{PuertaEmbarqueId}" : "Por asignar")}",
                $"Embarque  : {(TieneHoraEmbarque ? HoraEmbarque!.EnFormatoCorto : "Por confirmar")}",
                $"QR        : {CodigoQr}"
            };

            return string.Join(Environment.NewLine, lineas);
        }
    }

    public override string ToString() =>
        $"Pase [{CodigoPase}] | " +
        $"Puerta: {(TienePuertaAsignada ? $"#{PuertaEmbarqueId}" : "Sin asignar")} | " +
        $"Embarque: {(TieneHoraEmbarque ? HoraEmbarque!.EnFormatoCorto : "Sin confirmar")}";
}