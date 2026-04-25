// src/modules/route/Domain/ValueObjects/DistanciaKmRoute.cs
namespace AirTicketSystem.modules.route.Domain.ValueObjects;

public sealed class DistanciaKmRoute
{
    public int Valor { get; }

    private DistanciaKmRoute(int valor) => Valor = valor;

    public static DistanciaKmRoute Crear(int valor)
    {
        if (valor <= 0)
            throw new ArgumentException("La distancia en kilómetros debe ser mayor a 0.");

        if (valor > 20_000)
            throw new ArgumentException(
                "La distancia no puede superar 20.000 km " +
                "(máxima distancia posible en vuelo comercial).");

        return new DistanciaKmRoute(valor);
    }

    // Conversión útil para reportes
    public double EnMillas => Math.Round(Valor * 0.621371, 2);

    public override string ToString() => $"{Valor} km";
}