// src/modules/documenttype/Domain/aggregate/DocumentType.cs
using AirTicketSystem.modules.documenttype.Domain.ValueObjects;

namespace AirTicketSystem.modules.documenttype.Domain.aggregate;

public sealed class DocumentType
{
    public int Id { get; private set; }
    public DescripcionDocumentType Descripcion { get; private set; } = null!;

    private DocumentType() { }

    public static DocumentType Crear(string descripcion)
    {
        return new DocumentType
        {
            Descripcion = DescripcionDocumentType.Crear(descripcion)
        };
    }

    public static DocumentType Reconstituir(int id, string descripcion)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tipo de documento no es válido.");

        var documentType = Crear(descripcion);
        documentType.Id = id;
        return documentType;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tipo de documento no es válido.");

        Id = id;
    }

    public void ActualizarDescripcion(string descripcion)
    {
        Descripcion = DescripcionDocumentType.Crear(descripcion);
    }

    public override string ToString() => Descripcion.ToString();
}