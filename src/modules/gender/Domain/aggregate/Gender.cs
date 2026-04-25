// src/modules/gender/Domain/aggregate/Gender.cs
using AirTicketSystem.modules.gender.Domain.ValueObjects;

namespace AirTicketSystem.modules.gender.Domain.aggregate;

public sealed class Gender
{
    public int Id { get; private set; }
    public NombreGender Nombre { get; private set; } = null!;

    private Gender() { }

    public static Gender Crear(string nombre)
    {
        return new Gender
        {
            Nombre = NombreGender.Crear(nombre)
        };
    }

    public static Gender Reconstituir(int id, string nombre)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del género no es válido.");

        var gender = Crear(nombre);
        gender.Id = id;
        return gender;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del género no es válido.");

        Id = id;
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreGender.Crear(nombre);
    }

    public override string ToString() => Nombre.ToString();
}