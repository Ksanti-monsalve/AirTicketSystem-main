// src/modules/luggage/Domain/ValueObjects/AltoDeclaradoCmLuggage.cs
namespace AirTicketSystem.modules.luggage.Domain.ValueObjects;

public sealed class AltoDeclaradoCmLuggage
{
    public int Valor { get; }

    private AltoDeclaradoCmLuggage(int valor) => Valor = valor;

    public static AltoDeclaradoCmLuggage Crear(int valor)
    {
        if (valor <= 0)
            throw new ArgumentException(
                "El alto declarado debe ser mayor a 0 cm.");

        if (valor > 200)
            throw new ArgumentException(
                "El alto declarado no puede superar 200 cm.");

        return new AltoDeclaradoCmLuggage(valor);
    }

    public override string ToString() => $"{Valor} cm";
}