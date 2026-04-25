// src/modules/addresstype/Domain/aggregate/AddressType.cs
using AirTicketSystem.modules.addresstype.Domain.ValueObjects;

namespace AirTicketSystem.modules.addresstype.Domain.aggregate;

public sealed class AddressType
{
    public int Id { get; private set; }
    public DescripcionAddressType Descripcion { get; private set; } = null!;

    private AddressType() { }

    public static AddressType Crear(string descripcion)
    {
        return new AddressType
        {
            Descripcion = DescripcionAddressType.Crear(descripcion)
        };
    }

    public static AddressType Reconstituir(int id, string descripcion)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tipo de dirección no es válido.");

        var addressType = Crear(descripcion);
        addressType.Id = id;
        return addressType;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tipo de dirección no es válido.");

        Id = id;
    }

    public void ActualizarDescripcion(string descripcion)
    {
        Descripcion = DescripcionAddressType.Crear(descripcion);
    }

    public override string ToString() => Descripcion.ToString();
}