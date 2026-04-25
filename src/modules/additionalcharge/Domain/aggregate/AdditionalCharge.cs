// src/modules/additionalcharge/Domain/aggregate/AdditionalCharge.cs
using AirTicketSystem.modules.additionalcharge.Domain.ValueObjects;

namespace AirTicketSystem.modules.additionalcharge.Domain.aggregate;

public sealed class AdditionalCharge
{
    public int Id { get; private set; }
    public int ReservaId { get; private set; }
    public ConceptoAdditionalCharge Concepto { get; private set; } = null!;
    public MontoAdditionalCharge Monto { get; private set; } = null!;
    public FechaAdditionalCharge Fecha { get; private set; } = null!;

    private AdditionalCharge() { }

    public static AdditionalCharge Reconstituir(
        int id,
        int reservaId,
        string concepto,
        decimal monto,
        DateTime fecha)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del cargo adicional no es válido.");

        return new AdditionalCharge
        {
            Id        = id,
            ReservaId = reservaId,
            Concepto  = ConceptoAdditionalCharge.Crear(concepto),
            Monto     = MontoAdditionalCharge.Crear(monto),
            Fecha     = FechaAdditionalCharge.Crear(fecha)
        };
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del cargo adicional no es válido.");

        Id = id;
    }

    public static AdditionalCharge Crear(
        int reservaId,
        string concepto,
        decimal monto)
    {
        if (reservaId <= 0)
            throw new ArgumentException("La reserva es obligatoria.");

        return new AdditionalCharge
        {
            ReservaId = reservaId,
            Concepto  = ConceptoAdditionalCharge.Crear(concepto),
            Monto     = MontoAdditionalCharge.Crear(monto),
            Fecha     = FechaAdditionalCharge.Ahora()
        };
    }

    // Métodos de fábrica expresivos por tipo de cargo
    public static AdditionalCharge PorEquipaje(
        int reservaId,
        decimal monto)
        => Crear(reservaId, "Cargo por equipaje adicional", monto);

    public static AdditionalCharge PorCambioAsiento(
        int reservaId,
        decimal monto)
        => Crear(reservaId, "Cargo por cambio de asiento", monto);

    public static AdditionalCharge PorExcesoPeso(
        int reservaId,
        decimal monto)
        => Crear(reservaId, "Cargo por exceso de peso en equipaje", monto);

    public static AdditionalCharge PorCambioVuelo(
        int reservaId,
        decimal monto)
        => Crear(reservaId, "Cargo por cambio de vuelo", monto);

    public static AdditionalCharge PorServicioEspecial(
        int reservaId,
        string descripcionServicio,
        decimal monto)
    {
        if (string.IsNullOrWhiteSpace(descripcionServicio))
            throw new ArgumentException(
                "La descripción del servicio especial es obligatoria.");

        return Crear(reservaId, $"Servicio especial: {descripcionServicio}", monto);
    }

    // ── Propiedades de negocio ───────────────────────────────

    public bool EsPorEquipaje =>
        Concepto.Valor.Contains("equipaje", StringComparison.OrdinalIgnoreCase);

    public bool EsPorCambioAsiento =>
        Concepto.Valor.Contains("asiento", StringComparison.OrdinalIgnoreCase);

    public bool EsPorExcesoPeso =>
        Concepto.Valor.Contains("exceso de peso", StringComparison.OrdinalIgnoreCase);

    public override string ToString() =>
        $"Cargo [{Concepto}] — {Monto} | {Fecha.EnFormatoCorto}";
}