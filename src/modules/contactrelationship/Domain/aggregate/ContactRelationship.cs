// src/modules/contactrelationship/Domain/aggregate/ContactRelationship.cs
using AirTicketSystem.modules.contactrelationship.Domain.ValueObjects;

namespace AirTicketSystem.modules.contactrelationship.Domain.aggregate;

public sealed class ContactRelationship
{
    public int Id { get; private set; }
    public DescripcionContactRelationship Descripcion { get; private set; } = null!;

    private ContactRelationship() { }

    public static ContactRelationship Crear(string descripcion)
    {
        return new ContactRelationship
        {
            Descripcion = DescripcionContactRelationship.Crear(descripcion)
        };
    }

    public static ContactRelationship Reconstituir(int id, string descripcion)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la relación de contacto no es válido.");

        var contactRelationship = Crear(descripcion);
        contactRelationship.Id = id;
        return contactRelationship;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la relación de contacto no es válido.");

        Id = id;
    }

    public void ActualizarDescripcion(string descripcion)
    {
        Descripcion = DescripcionContactRelationship.Crear(descripcion);
    }

    public override string ToString() => Descripcion.ToString();
}