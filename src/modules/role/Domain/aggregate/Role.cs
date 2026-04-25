// src/modules/role/Domain/aggregate/Role.cs
using AirTicketSystem.modules.role.Domain.ValueObjects;

namespace AirTicketSystem.modules.role.Domain.aggregate;

public sealed class Role
{
    public int Id { get; private set; }
    public NombreRole Nombre { get; private set; } = null!;

    private Role() { }

    public static Role Reconstituir(int id, string nombre)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del rol no es válido.");

        return new Role { Id = id, Nombre = NombreRole.Crear(nombre) };
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del rol no es válido.");

        Id = id;
    }

    public static Role Crear(string nombre)
    {
        return new Role
        {
            Nombre = NombreRole.Crear(nombre)
        };
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreRole.Crear(nombre);
    }

    public override string ToString() => Nombre.ToString();
}