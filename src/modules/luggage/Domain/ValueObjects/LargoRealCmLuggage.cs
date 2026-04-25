// src/modules/luggage/Domain/ValueObjects/LargoRealCmLuggage.cs
namespace AirTicketSystem.modules.luggage.Domain.ValueObjects;

public sealed class LargoRealCmLuggage
{
    public int Valor { get; }

    private LargoRealCmLuggage(int valor) => Valor = valor;

    public static LargoRealCmLuggage Crear(int valor)
    {
        if (valor <= 0)
            throw new ArgumentException(
                "El largo real debe ser mayor a 0 cm.");

        if (valor > 300)
            throw new ArgumentException(
                "El largo real no puede superar 300 cm.");

        return new LargoRealCmLuggage(valor);
    }

    public bool ExcedeLimite(int largoMaximo) => Valor > largoMaximo;

    public override string ToString() => $"{Valor} cm";
}