// src/modules/route/Domain/ValueObjects/DuracionEstimadaMinRoute.cs
namespace AirTicketSystem.modules.route.Domain.ValueObjects;

public sealed class DuracionEstimadaMinRoute
{
    public int Valor { get; }

    private DuracionEstimadaMinRoute(int valor) => Valor = valor;

    public static DuracionEstimadaMinRoute Crear(int valor)
    {
        if (valor <= 0)
            throw new ArgumentException("La duración estimada debe ser mayor a 0 minutos.");

        // Vuelo comercial más largo del mundo: ~1100 minutos (~18 horas)
        if (valor > 1_200)
            throw new ArgumentException(
                "La duración estimada no puede superar 1.200 minutos (20 horas).");

        return new DuracionEstimadaMinRoute(valor);
    }

    // Conversiones útiles para pantallas de consola
    public int Horas => Valor / 60;
    public int MinutosRestantes => Valor % 60;
    public string EnFormatoHorasMinutos => $"{Horas}h {MinutosRestantes}min";

    public override string ToString() => EnFormatoHorasMinutos;
}