// src/modules/route/Domain/aggregate/Route.cs
using AirTicketSystem.modules.route.Domain.ValueObjects;

namespace AirTicketSystem.modules.route.Domain.aggregate;

public sealed class Route
{
    public int Id { get; private set; }
    public int AerolineaId { get; private set; }
    public int OrigenId { get; private set; }
    public int DestinoId { get; private set; }
    public DistanciaKmRoute? DistanciaKm { get; private set; }
    public DuracionEstimadaMinRoute? DuracionEstimadaMin { get; private set; }
    public ActivaRoute Activa { get; private set; } = null!;

    private Route() { }

    public static Route Crear(
        int aerolineaId,
        int origenId,
        int destinoId,
        int? distanciaKm = null,
        int? duracionEstimadaMin = null)
    {
        if (aerolineaId <= 0)
            throw new ArgumentException("La aerolínea es obligatoria.");

        if (origenId <= 0)
            throw new ArgumentException("El aeropuerto de origen es obligatorio.");

        if (destinoId <= 0)
            throw new ArgumentException("El aeropuerto de destino es obligatorio.");

        if (origenId == destinoId)
            throw new InvalidOperationException(
                "El aeropuerto de origen y destino no pueden ser el mismo.");

        return new Route
        {
            AerolineaId        = aerolineaId,
            OrigenId           = origenId,
            DestinoId          = destinoId,
            DistanciaKm        = distanciaKm is not null
                ? DistanciaKmRoute.Crear(distanciaKm.Value)
                : null,
            DuracionEstimadaMin = duracionEstimadaMin is not null
                ? DuracionEstimadaMinRoute.Crear(duracionEstimadaMin.Value)
                : null,
            Activa = ActivaRoute.Activa()
        };
    }

    public static Route Reconstituir(
        int id,
        int aerolineaId,
        int origenId,
        int destinoId,
        int? distanciaKm,
        int? duracionEstimadaMin,
        bool activa)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la ruta no es valido.");

        var route = Crear(
            aerolineaId,
            origenId,
            destinoId,
            distanciaKm,
            duracionEstimadaMin);

        route.Id = id;
        route.Activa = ActivaRoute.Crear(activa);
        return route;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la ruta no es valido.");

        Id = id;
    }

    public void ActualizarDistancia(int? distanciaKm)
    {
        DistanciaKm = distanciaKm is not null
            ? DistanciaKmRoute.Crear(distanciaKm.Value)
            : null;
    }

    public void ActualizarDuracion(int? duracionEstimadaMin)
    {
        DuracionEstimadaMin = duracionEstimadaMin is not null
            ? DuracionEstimadaMinRoute.Crear(duracionEstimadaMin.Value)
            : null;
    }

    public void Activar()
    {
        if (Activa.Valor)
            throw new InvalidOperationException(
                "La ruta ya se encuentra activa.");

        Activa = ActivaRoute.Activa();
    }

    public void Desactivar()
    {
        if (!Activa.Valor)
            throw new InvalidOperationException(
                "La ruta ya se encuentra inactiva.");

        Activa = ActivaRoute.Inactiva();
    }

    // Propiedades de negocio
    public bool EstaOperativa => Activa.Valor;

    public bool TieneInformacionCompleta =>
        DistanciaKm is not null && DuracionEstimadaMin is not null;

    public string DuracionFormateada =>
        DuracionEstimadaMin is not null
            ? DuracionEstimadaMin.EnFormatoHorasMinutos
            : "Duración no registrada";

    public override string ToString() =>
        $"Ruta {OrigenId} → {DestinoId} [{CodigoAerolinea()}]";

    private string CodigoAerolinea() =>
        $"Aerolínea #{AerolineaId}";
}