// src/modules/city/Domain/aggregate/City.cs
using AirTicketSystem.modules.city.Domain.ValueObjects;

namespace AirTicketSystem.modules.city.Domain.aggregate;

public sealed class City
{
    public int Id { get; private set; }
    public int DepartamentoId { get; private set; }
    public NombreCity Nombre { get; private set; } = null!;
    public CodigoPostalCity? CodigoPostal { get; private set; }

    private City() { }

    public static City Crear(int departamentoId, string nombre, string? codigoPostal = null)
    {
        if (departamentoId <= 0)
            throw new ArgumentException("El departamento es obligatorio.");

        return new City
        {
            DepartamentoId = departamentoId,
            Nombre         = NombreCity.Crear(nombre),
            CodigoPostal   = codigoPostal is not null
                ? CodigoPostalCity.Crear(codigoPostal)
                : null
        };
    }

    public static City Reconstituir(int id, int departamentoId, string nombre, string? codigoPostal)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la ciudad no es válido.");

        var city = Crear(departamentoId, nombre, codigoPostal);
        city.Id = id;
        return city;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la ciudad no es válido.");

        Id = id;
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreCity.Crear(nombre);
    }

    public void ActualizarCodigoPostal(string? codigoPostal)
    {
        CodigoPostal = codigoPostal is not null
            ? CodigoPostalCity.Crear(codigoPostal)
            : null;
    }

    public override string ToString() =>
        CodigoPostal is not null
            ? $"{Nombre} ({CodigoPostal})"
            : Nombre.ToString();
}
