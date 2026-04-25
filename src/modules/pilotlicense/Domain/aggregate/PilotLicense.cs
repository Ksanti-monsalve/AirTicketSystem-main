// src/modules/pilotlicense/Domain/aggregate/PilotLicense.cs
using AirTicketSystem.modules.pilotlicense.Domain.ValueObjects;

namespace AirTicketSystem.modules.pilotlicense.Domain.aggregate;

public sealed class PilotLicense
{
    public int Id { get; private set; }
    public int TrabajadorId { get; private set; }
    public NumeroLicenciaPilotLicense NumeroLicencia { get; private set; } = null!;
    public TipoLicenciaPilotLicense TipoLicencia { get; private set; } = null!;
    public FechaExpedicionPilotLicense FechaExpedicion { get; private set; } = null!;
    public FechaVencimientoPilotLicense FechaVencimiento { get; private set; } = null!;
    public AutoridadEmisoraPilotLicense AutoridadEmisora { get; private set; } = null!;
    public ActivaPilotLicense Activa { get; private set; } = null!;

    private PilotLicense() { }

    public static PilotLicense Reconstituir(
        int id,
        int trabajadorId,
        string numeroLicencia,
        string tipoLicencia,
        DateOnly fechaExpedicion,
        DateOnly fechaVencimiento,
        string autoridadEmisora,
        bool activa)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la licencia no es válido.");

        return new PilotLicense
        {
            Id               = id,
            TrabajadorId     = trabajadorId,
            NumeroLicencia   = NumeroLicenciaPilotLicense.Crear(numeroLicencia),
            TipoLicencia     = TipoLicenciaPilotLicense.Crear(tipoLicencia),
            FechaExpedicion  = FechaExpedicionPilotLicense.Crear(fechaExpedicion),
            FechaVencimiento = FechaVencimientoPilotLicense.Crear(fechaVencimiento),
            AutoridadEmisora = AutoridadEmisoraPilotLicense.Crear(autoridadEmisora),
            Activa           = activa
                ? ActivaPilotLicense.Activa()
                : ActivaPilotLicense.Inactiva()
        };
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la licencia no es válido.");

        Id = id;
    }

    public static PilotLicense Crear(
        int trabajadorId,
        string numeroLicencia,
        string tipoLicencia,
        DateOnly fechaExpedicion,
        DateOnly fechaVencimiento,
        string autoridadEmisora)
    {
        if (trabajadorId <= 0)
            throw new ArgumentException("El trabajador es obligatorio.");

        var expedicion  = FechaExpedicionPilotLicense.Crear(fechaExpedicion);
        var vencimiento = FechaVencimientoPilotLicense.Crear(fechaVencimiento);

        if (vencimiento.Valor <= expedicion.Valor)
            throw new InvalidOperationException(
                "La fecha de vencimiento debe ser posterior a la fecha de expedición.");

        return new PilotLicense
        {
            TrabajadorId     = trabajadorId,
            NumeroLicencia   = NumeroLicenciaPilotLicense.Crear(numeroLicencia),
            TipoLicencia     = TipoLicenciaPilotLicense.Crear(tipoLicencia),
            FechaExpedicion  = expedicion,
            FechaVencimiento = vencimiento,
            AutoridadEmisora = AutoridadEmisoraPilotLicense.Crear(autoridadEmisora),
            Activa           = ActivaPilotLicense.Activa()
        };
    }

    // ── Gestión de licencia ──────────────────────────────────

    public void Renovar(DateOnly nuevaFechaVencimiento)
    {
        var nuevaFecha = FechaVencimientoPilotLicense.Crear(nuevaFechaVencimiento);

        if (nuevaFecha.Valor <= FechaVencimiento.Valor)
            throw new InvalidOperationException(
                "La nueva fecha de vencimiento debe ser posterior a la actual.");

        FechaVencimiento = nuevaFecha;
        Activa           = ActivaPilotLicense.Activa();
    }

    public void Suspender()
    {
        if (!Activa.Valor)
            throw new InvalidOperationException(
                "La licencia ya se encuentra suspendida.");

        Activa = ActivaPilotLicense.Inactiva();
    }

    public void Reactivar()
    {
        if (Activa.Valor)
            throw new InvalidOperationException(
                "La licencia ya se encuentra activa.");

        if (!FechaVencimiento.EstaVigente)
            throw new InvalidOperationException(
                "No se puede reactivar una licencia vencida. Debe renovarse primero.");

        Activa = ActivaPilotLicense.Activa();
    }

    // ── Propiedades de negocio ───────────────────────────────

    public bool EstaVigente =>
        Activa.Valor && FechaVencimiento.EstaVigente;

    public bool VenceProximamente =>
        Activa.Valor && FechaVencimiento.VenceProximamente;

    public bool HabilitaVuelosComerciales =>
        EstaVigente && TipoLicencia.HabilitaVuelosComerciales;

    public int DiasHastaVencimiento =>
        FechaVencimiento.DiasRestantes;

    public override string ToString() =>
        $"[{TipoLicencia.Valor}] {NumeroLicencia} — " +
        $"{(EstaVigente ? "Vigente" : "Vencida")}";
}