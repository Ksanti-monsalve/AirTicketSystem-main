// src/modules/fare/Domain/ValueObjects/VigenteHastaFare.cs
namespace AirTicketSystem.modules.fare.Domain.ValueObjects;

public sealed class VigenteHastaFare
{
    public DateOnly Valor { get; }

    private VigenteHastaFare(DateOnly valor) => Valor = valor;

    public static VigenteHastaFare Crear(DateOnly valor)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);

        if (valor < hoy)
            throw new ArgumentException(
                "La fecha de vigencia no puede ser una fecha pasada.");

        if (valor > hoy.AddYears(5))
            throw new ArgumentException(
                "La vigencia de la tarifa no puede superar 5 años en el futuro.");

        return new VigenteHastaFare(valor);
    }

    public bool EstaVigente
    {
        get
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            return Valor >= hoy;
        }
    }

    public int DiasDeVigencia
    {
        get
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            return Valor.DayNumber - hoy.DayNumber;
        }
    }

    public bool VenceProximamente
    {
        get
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var diasRestantes = Valor.DayNumber - hoy.DayNumber;
            return diasRestantes >= 0 && diasRestantes <= 30;
        }
    }

    public override string ToString() => Valor.ToString("dd/MM/yyyy");
}