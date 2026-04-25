// src/modules/luggage/Domain/ValueObjects/AltoRealCmLuggage.cs
namespace AirTicketSystem.modules.luggage.Domain.ValueObjects;

public sealed class AltoRealCmLuggage
{
    public int Valor { get; }

    private AltoRealCmLuggage(int valor) => Valor = valor;

    public static AltoRealCmLuggage Crear(int valor)
    {
        if (valor <= 0)
            throw new ArgumentException(
                "El alto real debe ser mayor a 0 cm.");

        if (valor > 200)
            throw new ArgumentException(
                "El alto real no puede superar 200 cm.");

        return new AltoRealCmLuggage(valor);
    }

    public bool ExcedeLimite(int altoMaximo) => Valor > altoMaximo;

    public override string ToString() => $"{Valor} cm";
}