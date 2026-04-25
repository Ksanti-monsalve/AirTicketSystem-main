// src/modules/pilotlicense/Domain/ValueObjects/FechaExpedicionPilotLicense.cs
namespace AirTicketSystem.modules.pilotlicense.Domain.ValueObjects;

public sealed class FechaExpedicionPilotLicense
{
    public DateOnly Valor { get; }

    private FechaExpedicionPilotLicense(DateOnly valor) => Valor = valor;

    public static FechaExpedicionPilotLicense Crear(DateOnly valor)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);

        if (valor > hoy)
            throw new ArgumentException(
                "La fecha de expedición de la licencia no puede ser una fecha futura.");

        if (valor < new DateOnly(1950, 1, 1))
            throw new ArgumentException(
                "La fecha de expedición no puede ser anterior a 1950.");

        return new FechaExpedicionPilotLicense(valor);
    }

    public override string ToString() => Valor.ToString("dd/MM/yyyy");
}