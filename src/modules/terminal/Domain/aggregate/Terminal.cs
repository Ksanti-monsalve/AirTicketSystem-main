// src/modules/terminal/Domain/aggregate/Terminal.cs
using AirTicketSystem.modules.terminal.Domain.ValueObjects;

namespace AirTicketSystem.modules.terminal.Domain.aggregate;

public sealed class Terminal
{
    public int Id { get; private set; }
    public int AeropuertoId { get; private set; }
    public NombreTerminal Nombre { get; private set; } = null!;
    public DescripcionTerminal? Descripcion { get; private set; }

    private Terminal() { }

    public static Terminal Crear(
        int aeropuertoId,
        string nombre,
        string? descripcion = null)
    {
        if (aeropuertoId <= 0)
            throw new ArgumentException("El aeropuerto es obligatorio.");

        return new Terminal
        {
            AeropuertoId = aeropuertoId,
            Nombre       = NombreTerminal.Crear(nombre),
            Descripcion  = descripcion is not null
                ? DescripcionTerminal.Crear(descripcion)
                : null
        };
    }

    public static Terminal Reconstituir(
        int id,
        int aeropuertoId,
        string nombre,
        string? descripcion = null)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la terminal no es valido.");

        var terminal = Crear(aeropuertoId, nombre, descripcion);
        terminal.Id = id;
        return terminal;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la terminal no es valido.");

        Id = id;
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreTerminal.Crear(nombre);
    }

    public void ActualizarDescripcion(string? descripcion)
    {
        Descripcion = descripcion is not null
            ? DescripcionTerminal.Crear(descripcion)
            : null;
    }

    public override string ToString() => Nombre.ToString();
}