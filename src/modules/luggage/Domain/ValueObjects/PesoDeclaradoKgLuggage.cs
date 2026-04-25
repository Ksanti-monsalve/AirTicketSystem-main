// src/modules/luggage/Domain/ValueObjects/PesoDeclaradoKgLuggage.cs
namespace AirTicketSystem.modules.luggage.Domain.ValueObjects;

public sealed class PesoDeclaradoKgLuggage
{
    public decimal Valor { get; }

    private PesoDeclaradoKgLuggage(decimal valor) => Valor = valor;

    public static PesoDeclaradoKgLuggage Crear(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException(
                "El peso declarado debe ser mayor a 0 kg.");

        // Límite práctico para equipaje declarado por pasajero
        if (valor > 50)
            throw new ArgumentException(
                "El peso declarado no puede superar 50 kg por pieza.");

        return new PesoDeclaradoKgLuggage(Math.Round(valor, 2));
    }

    public override string ToString() => $"{Valor} kg";
}