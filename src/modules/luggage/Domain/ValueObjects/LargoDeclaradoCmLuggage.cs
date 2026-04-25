// src/modules/luggage/Domain/ValueObjects/LargoDeclaradoCmLuggage.cs
namespace AirTicketSystem.modules.luggage.Domain.ValueObjects;

public sealed class LargoDeclaradoCmLuggage
{
    public int Valor { get; }

    private LargoDeclaradoCmLuggage(int valor) => Valor = valor;

    public static LargoDeclaradoCmLuggage Crear(int valor)
    {
        if (valor <= 0)
            throw new ArgumentException(
                "El largo declarado debe ser mayor a 0 cm.");

        if (valor > 300)
            throw new ArgumentException(
                "El largo declarado no puede superar 300 cm.");

        return new LargoDeclaradoCmLuggage(valor);
    }

    public override string ToString() => $"{Valor} cm";
}