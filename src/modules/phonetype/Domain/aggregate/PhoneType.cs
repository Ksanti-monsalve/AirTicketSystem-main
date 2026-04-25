// src/modules/phonetype/Domain/aggregate/PhoneType.cs
using AirTicketSystem.modules.phonetype.Domain.ValueObjects;

namespace AirTicketSystem.modules.phonetype.Domain.aggregate;

public sealed class PhoneType
{
    public int Id { get; private set; }
    public DescripcionPhoneType Descripcion { get; private set; } = null!;

    private PhoneType() { }

    public static PhoneType Crear(string descripcion)
    {
        return new PhoneType
        {
            Descripcion = DescripcionPhoneType.Crear(descripcion)
        };
    }

    public static PhoneType Reconstituir(int id, string descripcion)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tipo de teléfono no es válido.");

        var phoneType = Crear(descripcion);
        phoneType.Id = id;
        return phoneType;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tipo de teléfono no es válido.");

        Id = id;
    }

    public void ActualizarDescripcion(string descripcion)
    {
        Descripcion = DescripcionPhoneType.Crear(descripcion);
    }

    public override string ToString() => Descripcion.ToString();
}