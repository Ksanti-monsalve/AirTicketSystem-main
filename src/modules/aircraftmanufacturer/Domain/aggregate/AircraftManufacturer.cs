// src/modules/aircraftmanufacturer/Domain/aggregate/AircraftManufacturer.cs
using AirTicketSystem.modules.aircraftmanufacturer.Domain.ValueObjects;

namespace AirTicketSystem.modules.aircraftmanufacturer.Domain.aggregate;

public sealed class AircraftManufacturer
{
    public int Id { get; private set; }
    public int PaisId { get; private set; }
    public NombreAircraftManufacturer Nombre { get; private set; } = null!;
    public SitioWebAircraftManufacturer? SitioWeb { get; private set; }

    private AircraftManufacturer() { }

    public static AircraftManufacturer Crear(
        int paisId,
        string nombre,
        string? sitioWeb = null)
    {
        if (paisId <= 0)
            throw new ArgumentException("El país del fabricante es obligatorio.");

        return new AircraftManufacturer
        {
            PaisId   = paisId,
            Nombre   = NombreAircraftManufacturer.Crear(nombre),
            SitioWeb = sitioWeb is not null
                ? SitioWebAircraftManufacturer.Crear(sitioWeb)
                : null
        };
    }

    public static AircraftManufacturer Reconstituir(
        int id,
        int paisId,
        string nombre,
        string? sitioWeb)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del fabricante no es valido.");

        var manufacturer = Crear(paisId, nombre, sitioWeb);
        manufacturer.Id = id;
        return manufacturer;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del fabricante no es valido.");

        Id = id;
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreAircraftManufacturer.Crear(nombre);
    }

    public void ActualizarSitioWeb(string? sitioWeb)
    {
        SitioWeb = sitioWeb is not null
            ? SitioWebAircraftManufacturer.Crear(sitioWeb)
            : null;
    }

    public void ActualizarPais(int paisId)
    {
        if (paisId <= 0)
            throw new ArgumentException("El país es obligatorio.");

        PaisId = paisId;
    }

    public override string ToString() => Nombre.ToString();
}