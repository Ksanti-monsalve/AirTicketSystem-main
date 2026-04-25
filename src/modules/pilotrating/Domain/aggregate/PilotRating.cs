// src/modules/pilotrating/Domain/aggregate/PilotRating.cs
using AirTicketSystem.modules.pilotrating.Domain.ValueObjects;

namespace AirTicketSystem.modules.pilotrating.Domain.aggregate;

public sealed class PilotRating
{
    public int Id { get; private set; }
    public int LicenciaId { get; private set; }
    public int ModeloAvionId { get; private set; }
    public FechaHabilitacionPilotRating FechaHabilitacion { get; private set; } = null!;
    public FechaVencimientoPilotRating FechaVencimiento { get; private set; } = null!;

    private PilotRating() { }

    public static PilotRating Crear(
        int licenciaId,
        int modeloAvionId,
        DateOnly fechaHabilitacion,
        DateOnly fechaVencimiento)
    {
        if (licenciaId <= 0)
            throw new ArgumentException("La licencia es obligatoria.");

        if (modeloAvionId <= 0)
            throw new ArgumentException("El modelo de avión es obligatorio.");

        var habilitacion = FechaHabilitacionPilotRating.Crear(fechaHabilitacion);
        var vencimiento  = FechaVencimientoPilotRating.Crear(fechaVencimiento);

        if (vencimiento.Valor <= habilitacion.Valor)
            throw new InvalidOperationException(
                "La fecha de vencimiento debe ser posterior a la fecha de habilitación.");

        return new PilotRating
        {
            LicenciaId        = licenciaId,
            ModeloAvionId     = modeloAvionId,
            FechaHabilitacion = habilitacion,
            FechaVencimiento  = vencimiento
        };
    }

    public static PilotRating Reconstituir(
        int id,
        int licenciaId,
        int modeloAvionId,
        DateOnly fechaHabilitacion,
        DateOnly fechaVencimiento)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la habilitación no es válido.");

        return new PilotRating
        {
            Id                = id,
            LicenciaId        = licenciaId,
            ModeloAvionId     = modeloAvionId,
            FechaHabilitacion = FechaHabilitacionPilotRating.Crear(fechaHabilitacion),
            FechaVencimiento  = FechaVencimientoPilotRating.Crear(fechaVencimiento)
        };
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la habilitación no es válido.");

        Id = id;
    }

    public void Renovar(DateOnly nuevaFechaVencimiento)
    {
        var nuevaFecha = FechaVencimientoPilotRating.Crear(nuevaFechaVencimiento);

        if (nuevaFecha.Valor <= FechaVencimiento.Valor)
            throw new InvalidOperationException(
                "La nueva fecha de vencimiento debe ser posterior a la actual.");

        FechaVencimiento = nuevaFecha;
    }

    // ── Propiedades de negocio ───────────────────────────────

    public bool EstaVigente => FechaVencimiento.EstaVigente;

    public bool VenceProximamente => FechaVencimiento.VenceProximamente;

    public int DiasHastaVencimiento => FechaVencimiento.DiasRestantes;

    public bool HabilitaParaVolar => EstaVigente;

    public override string ToString() =>
        $"Habilitación Modelo #{ModeloAvionId} — " +
        $"{(EstaVigente ? "Vigente" : "Vencida")} " +
        $"({DiasHastaVencimiento} días)";
}
