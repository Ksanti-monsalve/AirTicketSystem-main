// src/modules/airport/Domain/aggregate/Airport.cs
using AirTicketSystem.modules.airport.Domain.ValueObjects;

namespace AirTicketSystem.modules.airport.Domain.aggregate;

public sealed class Airport
{
    public int Id { get; private set; }
    public int CiudadId { get; private set; }
    public CodigoIataAirport CodigoIata { get; private set; } = null!;
    public CodigoIcaoAirport CodigoIcao { get; private set; } = null!;
    public NombreAirport Nombre { get; private set; } = null!;
    public DireccionAirport? Direccion { get; private set; }
    public ActivoAirport Activo { get; private set; } = null!;

    private Airport() { }

    public static Airport Crear(
        int ciudadId,
        string codigoIata,
        string codigoIcao,
        string nombre,
        string? direccion = null)
    {
        if (ciudadId <= 0)
            throw new ArgumentException("La ciudad es obligatoria.");

        return new Airport
        {
            CiudadId   = ciudadId,
            CodigoIata = CodigoIataAirport.Crear(codigoIata),
            CodigoIcao = CodigoIcaoAirport.Crear(codigoIcao),
            Nombre     = NombreAirport.Crear(nombre),
            Direccion  = direccion is not null
                ? DireccionAirport.Crear(direccion)
                : null,
            Activo = ActivoAirport.Activo()
        };
    }

    public static Airport Reconstituir(
        int id,
        int ciudadId,
        string codigoIata,
        string codigoIcao,
        string nombre,
        string? direccion,
        bool activo)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del aeropuerto no es valido.");

        var airport = Crear(ciudadId, codigoIata, codigoIcao, nombre, direccion);
        airport.Id = id;
        airport.Activo = ActivoAirport.Crear(activo);
        return airport;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del aeropuerto no es valido.");

        Id = id;
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreAirport.Crear(nombre);
    }

    public void ActualizarDireccion(string? direccion)
    {
        Direccion = direccion is not null
            ? DireccionAirport.Crear(direccion)
            : null;
    }

    public void Activar() => Activo = ActivoAirport.Activo();

    public void Desactivar() => Activo = ActivoAirport.Inactivo();

    public override string ToString() =>
        $"[{CodigoIata}] {Nombre}";
}