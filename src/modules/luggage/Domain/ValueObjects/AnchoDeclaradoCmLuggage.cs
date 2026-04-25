// src/modules/luggage/Domain/ValueObjects/AnchoDeclaradoCmLuggage.cs
namespace AirTicketSystem.modules.luggage.Domain.ValueObjects;

public sealed class AnchoDeclaradoCmLuggage
{
    public int Valor { get; }

    private AnchoDeclaradoCmLuggage(int valor) => Valor = valor;

    public static AnchoDeclaradoCmLuggage Crear(int valor)
    {
        if (valor <= 0)
            throw new ArgumentException(
                "El ancho declarado debe ser mayor a 0 cm.");

        if (valor > 200)
            throw new ArgumentException(
                "El ancho declarado no puede superar 200 cm.");

        return new AnchoDeclaradoCmLuggage(valor);
    }

    public override string ToString() => $"{Valor} cm";
}