// src/modules/serviceclass/Domain/aggregate/ServiceClass.cs
using AirTicketSystem.modules.serviceclass.Domain.ValueObjects;

namespace AirTicketSystem.modules.serviceclass.Domain.aggregate;

public sealed class ServiceClass
{
    public int Id { get; private set; }
    public NombreServiceClass Nombre { get; private set; } = null!;
    public CodigoServiceClass Codigo { get; private set; } = null!;
    public DescripcionServiceClass? Descripcion { get; private set; }

    private ServiceClass() { }

    public static ServiceClass Crear(
        string nombre,
        string codigo,
        string? descripcion = null)
    {
        return new ServiceClass
        {
            Nombre      = NombreServiceClass.Crear(nombre),
            Codigo      = CodigoServiceClass.Crear(codigo),
            Descripcion = descripcion is not null
                ? DescripcionServiceClass.Crear(descripcion)
                : null
        };
    }

    public static ServiceClass Reconstituir(
        int id,
        string nombre,
        string codigo,
        string? descripcion)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la clase de servicio no es valido.");

        var serviceClass = Crear(nombre, codigo, descripcion);
        serviceClass.Id = id;
        return serviceClass;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la clase de servicio no es valido.");

        Id = id;
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreServiceClass.Crear(nombre);
    }

    public void ActualizarDescripcion(string? descripcion)
    {
        Descripcion = descripcion is not null
            ? DescripcionServiceClass.Crear(descripcion)
            : null;
    }

    public override string ToString() =>
        $"[{Codigo}] {Nombre}";
}