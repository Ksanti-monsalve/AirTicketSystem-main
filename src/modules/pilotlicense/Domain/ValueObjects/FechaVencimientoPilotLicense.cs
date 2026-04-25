// src/modules/pilotlicense/Domain/ValueObjects/FechaVencimientoPilotLicense.cs
namespace AirTicketSystem.modules.pilotlicense.Domain.ValueObjects;

public sealed class FechaVencimientoPilotLicense
{
    public DateOnly Valor { get; }

    private FechaVencimientoPilotLicense(DateOnly valor) => Valor = valor;

    public static FechaVencimientoPilotLicense Crear(DateOnly valor)
    {
        if (valor < new DateOnly(2000, 1, 1))
            throw new ArgumentException(
                "La fecha de vencimiento de la licencia no puede ser anterior al año 2000.");

        // Licencias no se expiden a más de 10 años
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        if (valor > hoy.AddYears(10))
            throw new ArgumentException(
                "La fecha de vencimiento no puede superar 10 años en el futuro.");

        return new FechaVencimientoPilotLicense(valor);
    }

    public bool EstaVigente
    {
        get
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            return Valor >= hoy;
        }
    }

    public bool VenceProximamente
    {
        get
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var diasRestantes = Valor.DayNumber - hoy.DayNumber;
            return diasRestantes >= 0 && diasRestantes <= 60;
        }
    }

    public int DiasRestantes
    {
        get
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            return Valor.DayNumber - hoy.DayNumber;
        }
    }

    public override string ToString() => Valor.ToString("dd/MM/yyyy");
}