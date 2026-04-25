// src/modules/country/Domain/aggregate/Country.cs
using AirTicketSystem.modules.country.Domain.ValueObjects;

namespace AirTicketSystem.modules.country.Domain.aggregate;

public sealed class Country
{
    public int Id { get; private set; }
    public int ContinenteId { get; private set; }
    public NombreCountry Nombre { get; private set; } = null!;
    public CodigoIso2Country CodigoIso2 { get; private set; } = null!;
    public CodigoIso3Country CodigoIso3 { get; private set; } = null!;

    private Country() { }

    public static Country Crear(
        int continenteId,
        string nombre,
        string codigoIso2,
        string codigoIso3)
    {
        if (continenteId <= 0)
            throw new ArgumentException("El continente es obligatorio.");

        return new Country
        {
            ContinenteId = continenteId,
            Nombre       = NombreCountry.Crear(nombre),
            CodigoIso2   = CodigoIso2Country.Crear(codigoIso2),
            CodigoIso3   = CodigoIso3Country.Crear(codigoIso3)
        };
    }

    public static Country Reconstituir(
        int id,
        int continenteId,
        string nombre,
        string codigoIso2,
        string codigoIso3)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del país no es válido.");

        var country = Crear(continenteId, nombre, codigoIso2, codigoIso3);
        country.Id = id;
        return country;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del país no es válido.");

        Id = id;
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreCountry.Crear(nombre);
    }

    public void ActualizarCodigoIso2(string codigoIso2)
    {
        CodigoIso2 = CodigoIso2Country.Crear(codigoIso2);
    }

    public void ActualizarCodigoIso3(string codigoIso3)
    {
        CodigoIso3 = CodigoIso3Country.Crear(codigoIso3);
    }

    public override string ToString() =>
        $"[{CodigoIso2}] {Nombre}";
}
