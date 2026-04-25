// src/modules/person/Domain/aggregate/Person.cs
using AirTicketSystem.modules.person.Domain.ValueObjects;

namespace AirTicketSystem.modules.person.Domain.aggregate;

public sealed class Person
{
    public int Id { get; private set; }
    public int TipoDocId { get; private set; }
    public int? GeneroId { get; private set; }
    public int? NacionalidadId { get; private set; }
    public NumeroDocPerson NumeroDoc { get; private set; } = null!;
    public NombresPerson Nombres { get; private set; } = null!;
    public ApellidosPerson Apellidos { get; private set; } = null!;
    public FechaNacimientoPerson? FechaNacimiento { get; private set; }

    private Person() { }

    public static Person Crear(
        int tipoDocId,
        string numeroDoc,
        string nombres,
        string apellidos,
        DateOnly? fechaNacimiento = null,
        int? generoId = null,
        int? nacionalidadId = null)
    {
        if (tipoDocId <= 0)
            throw new ArgumentException("El tipo de documento es obligatorio.");

        return new Person
        {
            TipoDocId       = tipoDocId,
            NumeroDoc       = NumeroDocPerson.Crear(numeroDoc),
            Nombres         = NombresPerson.Crear(nombres),
            Apellidos       = ApellidosPerson.Crear(apellidos),
            FechaNacimiento = fechaNacimiento is not null
                ? FechaNacimientoPerson.Crear(fechaNacimiento.Value)
                : null,
            GeneroId       = generoId,
            NacionalidadId = nacionalidadId
        };
    }

    public static Person Reconstituir(
        int id,
        int tipoDocId,
        string numeroDoc,
        string nombres,
        string apellidos,
        DateOnly? fechaNacimiento,
        int? generoId,
        int? nacionalidadId)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la persona no es válido.");

        var person = Crear(tipoDocId, numeroDoc, nombres, apellidos,
            fechaNacimiento, generoId, nacionalidadId);
        person.Id = id;
        return person;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la persona no es válido.");

        Id = id;
    }

    public void ActualizarNombre(string nombres, string apellidos)
    {
        Nombres   = NombresPerson.Crear(nombres);
        Apellidos = ApellidosPerson.Crear(apellidos);
    }

    public void ActualizarFechaNacimiento(DateOnly? fechaNacimiento)
    {
        FechaNacimiento = fechaNacimiento is not null
            ? FechaNacimientoPerson.Crear(fechaNacimiento.Value)
            : null;
    }

    public void ActualizarGenero(int? generoId)
    {
        GeneroId = generoId;
    }

    public void ActualizarNacionalidad(int? nacionalidadId)
    {
        NacionalidadId = nacionalidadId;
    }

    // Utilidades de negocio
    public string NombreCompleto => $"{Nombres} {Apellidos}";

    public bool EsMenorDeEdad =>
        FechaNacimiento is not null && FechaNacimiento.EsMenorDeEdad;

    public bool EsInfante =>
        FechaNacimiento is not null && FechaNacimiento.EsInfante;

    public override string ToString() =>
        $"{NombreCompleto} — {NumeroDoc}";
}