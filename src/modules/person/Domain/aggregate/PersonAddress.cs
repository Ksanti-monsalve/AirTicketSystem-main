// src/modules/person/Domain/aggregate/PersonAddress.cs
using AirTicketSystem.modules.person.Domain.ValueObjects;

namespace AirTicketSystem.modules.person.Domain.aggregate;

public sealed class PersonAddress
{
    public int Id { get; private set; }
    public int PersonaId { get; private set; }
    public int TipoDireccionId { get; private set; }
    public int CiudadId { get; private set; }
    public DireccionLinea1PersonAddress DireccionLinea1 { get; private set; } = null!;
    public DireccionLinea2PersonAddress? DireccionLinea2 { get; private set; }
    public CodigoPostalPersonAddress? CodigoPostal { get; private set; }
    public EsPrincipalPersonAddress EsPrincipal { get; private set; } = null!;

    private PersonAddress() { }

    public static PersonAddress Crear(
        int personaId,
        int tipoDireccionId,
        int ciudadId,
        string direccionLinea1,
        string? direccionLinea2 = null,
        string? codigoPostal = null,
        bool esPrincipal = false)
    {
        if (personaId <= 0)
            throw new ArgumentException("La persona es obligatoria.");

        if (tipoDireccionId <= 0)
            throw new ArgumentException("El tipo de dirección es obligatorio.");

        if (ciudadId <= 0)
            throw new ArgumentException("La ciudad es obligatoria.");

        return new PersonAddress
        {
            PersonaId       = personaId,
            TipoDireccionId = tipoDireccionId,
            CiudadId        = ciudadId,
            DireccionLinea1 = DireccionLinea1PersonAddress.Crear(direccionLinea1),
            DireccionLinea2 = direccionLinea2 is not null
                ? DireccionLinea2PersonAddress.Crear(direccionLinea2)
                : null,
            CodigoPostal = codigoPostal is not null
                ? CodigoPostalPersonAddress.Crear(codigoPostal)
                : null,
            EsPrincipal = EsPrincipalPersonAddress.Crear(esPrincipal)
        };
    }

    public static PersonAddress Reconstituir(
        int id,
        int personaId,
        int tipoDireccionId,
        int ciudadId,
        string direccionLinea1,
        string? direccionLinea2,
        string? codigoPostal,
        bool esPrincipal)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la dirección no es válido.");

        var addr = Crear(personaId, tipoDireccionId, ciudadId,
            direccionLinea1, direccionLinea2, codigoPostal, esPrincipal);
        addr.Id = id;
        return addr;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la dirección no es válido.");

        Id = id;
    }

    public void ActualizarDireccion(
        string direccionLinea1,
        string? direccionLinea2 = null,
        string? codigoPostal = null)
    {
        DireccionLinea1 = DireccionLinea1PersonAddress.Crear(direccionLinea1);
        DireccionLinea2 = direccionLinea2 is not null
            ? DireccionLinea2PersonAddress.Crear(direccionLinea2)
            : null;
        CodigoPostal = codigoPostal is not null
            ? CodigoPostalPersonAddress.Crear(codigoPostal)
            : null;
    }

    public void ActualizarCiudad(int ciudadId)
    {
        if (ciudadId <= 0)
            throw new ArgumentException("La ciudad es obligatoria.");

        CiudadId = ciudadId;
    }

    public void MarcarComoPrincipal()
    {
        EsPrincipal = EsPrincipalPersonAddress.Principal();
    }

    public void MarcarComoSecundario()
    {
        EsPrincipal = EsPrincipalPersonAddress.Secundario();
    }

    public string DireccionCompleta =>
        DireccionLinea2 is not null
            ? $"{DireccionLinea1}, {DireccionLinea2}"
            : DireccionLinea1.ToString();

    public override string ToString() => DireccionCompleta;
}