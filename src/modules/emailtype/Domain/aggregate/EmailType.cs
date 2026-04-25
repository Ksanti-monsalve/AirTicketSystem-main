// src/modules/emailtype/Domain/aggregate/EmailType.cs
using AirTicketSystem.modules.emailtype.Domain.ValueObjects;

namespace AirTicketSystem.modules.emailtype.Domain.aggregate;

public sealed class EmailType
{
    public int Id { get; private set; }
    public DescripcionEmailType Descripcion { get; private set; } = null!;

    private EmailType() { }

    public static EmailType Crear(string descripcion)
    {
        return new EmailType
        {
            Descripcion = DescripcionEmailType.Crear(descripcion)
        };
    }

    public static EmailType Reconstituir(int id, string descripcion)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tipo de email no es válido.");

        var emailType = Crear(descripcion);
        emailType.Id = id;
        return emailType;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tipo de email no es válido.");

        Id = id;
    }

    public void ActualizarDescripcion(string descripcion)
    {
        Descripcion = DescripcionEmailType.Crear(descripcion);
    }

    public override string ToString() => Descripcion.ToString();
}