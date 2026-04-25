// src/modules/airline/Domain/aggregate/Airline.cs
using AirTicketSystem.modules.airline.Domain.ValueObjects;

namespace AirTicketSystem.modules.airline.Domain.aggregate;

public sealed class Airline
{
    public int Id { get; private set; }
    public int PaisId { get; private set; }
    public CodigoIataAerolinea CodigoIata { get; private set; } = null!;
    public CodigoIcaoAerolinea CodigoIcao { get; private set; } = null!;
    public NombreAerolinea Nombre { get; private set; } = null!;
    public NombreComercialAerolinea? NombreComercial { get; private set; }
    public SitioWebAerolinea? SitioWeb { get; private set; }
    public ActivaAerolinea Activa { get; private set; } = null!;

    private Airline() { }

    public static Airline Crear(
        int paisId,
        string codigoIata,
        string codigoIcao,
        string nombre,
        string? nombreComercial = null,
        string? sitioWeb = null)
    {
        if (paisId <= 0)
            throw new ArgumentException("El país de la aerolínea es obligatorio.");

        return new Airline
        {
            PaisId          = paisId,
            CodigoIata      = CodigoIataAerolinea.Crear(codigoIata),
            CodigoIcao      = CodigoIcaoAerolinea.Crear(codigoIcao),
            Nombre          = NombreAerolinea.Crear(nombre),
            NombreComercial = nombreComercial is not null
                ? NombreComercialAerolinea.Crear(nombreComercial)
                : null,
            SitioWeb = sitioWeb is not null
                ? SitioWebAerolinea.Crear(sitioWeb)
                : null,
            Activa = ActivaAerolinea.Activa()
        };
    }

    public static Airline Reconstituir(
        int id,
        int paisId,
        string codigoIata,
        string codigoIcao,
        string nombre,
        string? nombreComercial,
        string? sitioWeb,
        bool activa)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la aerolinea no es valido.");

        var airline = Crear(
            paisId,
            codigoIata,
            codigoIcao,
            nombre,
            nombreComercial,
            sitioWeb);

        airline.Id = id;
        airline.Activa = ActivaAerolinea.Crear(activa);
        return airline;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la aerolinea no es valido.");

        Id = id;
    }

    public void ActualizarNombre(string nombre, string? nombreComercial = null)
    {
        Nombre = NombreAerolinea.Crear(nombre);
        NombreComercial = nombreComercial is not null
            ? NombreComercialAerolinea.Crear(nombreComercial)
            : null;
    }

    public void ActualizarSitioWeb(string? sitioWeb)
    {
        SitioWeb = sitioWeb is not null
            ? SitioWebAerolinea.Crear(sitioWeb)
            : null;
    }

    public void ActualizarPais(int paisId)
    {
        if (paisId <= 0)
            throw new ArgumentException("El país es obligatorio.");

        PaisId = paisId;
    }

    public void Activar()
    {
        if (Activa.Valor)
            throw new InvalidOperationException(
                "La aerolínea ya se encuentra activa.");

        Activa = ActivaAerolinea.Activa();
    }

    public void Desactivar()
    {
        if (!Activa.Valor)
            throw new InvalidOperationException(
                "La aerolínea ya se encuentra inactiva.");

        Activa = ActivaAerolinea.Inactiva();
    }

    // Propiedades de negocio
    public bool EstaOperativa => Activa.Valor;

    public string NombreParaMostrar =>
        NombreComercial is not null
            ? NombreComercial.Valor
            : Nombre.Valor;

    public override string ToString() =>
        $"[{CodigoIata}] {NombreParaMostrar}";
}