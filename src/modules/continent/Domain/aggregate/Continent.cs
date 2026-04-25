// src/modules/continent/Domain/aggregate/Continent.cs
using AirTicketSystem.modules.continent.Domain.ValueObjects;

namespace AirTicketSystem.modules.continent.Domain.aggregate;

public sealed class Continent
{
    public int Id { get; private set; }
    public NombreContinent Nombre { get; private set; } = null!;
    public CodigoContinent Codigo { get; private set; } = null!;

    private Continent() { }

    public static Continent Crear(string nombre, string codigo)
    {
        return new Continent
        {
            Nombre = NombreContinent.Crear(nombre),
            Codigo = CodigoContinent.Crear(codigo)
        };
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreContinent.Crear(nombre);
    }

    public static Continent Reconstituir(int id, string nombre, string codigo)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del continente no es válido.");

        var continent = Crear(nombre, codigo);
        continent.Id = id;
        return continent;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del continente no es válido.");

        Id = id;
    }

    public void ActualizarCodigo(string codigo)
    {
        Codigo = CodigoContinent.Crear(codigo);
    }

    public override string ToString() =>
        $"[{Codigo}] {Nombre}";
}