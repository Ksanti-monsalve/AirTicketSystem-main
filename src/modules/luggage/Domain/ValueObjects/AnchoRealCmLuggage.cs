// src/modules/luggage/Domain/ValueObjects/AnchoRealCmLuggage.cs
namespace AirTicketSystem.modules.luggage.Domain.ValueObjects;

public sealed class AnchoRealCmLuggage
{
    public int Valor { get; }

    private AnchoRealCmLuggage(int valor) => Valor = valor;

    public static AnchoRealCmLuggage Crear(int valor)
    {
        if (valor <= 0)
            throw new ArgumentException(
                "El ancho real debe ser mayor a 0 cm.");

        if (valor > 200)
            throw new ArgumentException(
                "El ancho real no puede superar 200 cm.");

        return new AnchoRealCmLuggage(valor);
    }

    public bool ExcedeLimite(int anchoMaximo) => Valor > anchoMaximo;

    public override string ToString() => $"{Valor} cm";
}