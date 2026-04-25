// src/modules/aircraftmodel/Domain/aggregate/AircraftModel.cs
using AirTicketSystem.modules.aircraftmodel.Domain.ValueObjects;

namespace AirTicketSystem.modules.aircraftmodel.Domain.aggregate;

public sealed class AircraftModel
{
    public int Id { get; private set; }
    public int FabricanteId { get; private set; }
    public NombreAircraftModel Nombre { get; private set; } = null!;
    public CodigoModeloAircraftModel CodigoModelo { get; private set; } = null!;
    public AutonomiKmAircraftModel? AutonomiKm { get; private set; }
    public VelocidadCruceroKmhAircraftModel? VelocidadCruceroKmh { get; private set; }
    public DescripcionAircraftModel? Descripcion { get; private set; }

    private AircraftModel() { }

    public static AircraftModel Crear(
        int fabricanteId,
        string nombre,
        string codigoModelo,
        int? autonomiKm = null,
        int? velocidadCruceroKmh = null,
        string? descripcion = null)
    {
        if (fabricanteId <= 0)
            throw new ArgumentException("El fabricante es obligatorio.");

        return new AircraftModel
        {
            FabricanteId       = fabricanteId,
            Nombre             = NombreAircraftModel.Crear(nombre),
            CodigoModelo       = CodigoModeloAircraftModel.Crear(codigoModelo),
            AutonomiKm         = autonomiKm is not null
                ? AutonomiKmAircraftModel.Crear(autonomiKm.Value)
                : null,
            VelocidadCruceroKmh = velocidadCruceroKmh is not null
                ? VelocidadCruceroKmhAircraftModel.Crear(velocidadCruceroKmh.Value)
                : null,
            Descripcion = descripcion is not null
                ? DescripcionAircraftModel.Crear(descripcion)
                : null
        };
    }

    public static AircraftModel Reconstituir(
        int id,
        int fabricanteId,
        string nombre,
        string codigoModelo,
        int? autonomiKm,
        int? velocidadCruceroKmh,
        string? descripcion)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del modelo no es valido.");

        var model = Crear(
            fabricanteId,
            nombre,
            codigoModelo,
            autonomiKm,
            velocidadCruceroKmh,
            descripcion);

        model.Id = id;
        return model;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del modelo no es valido.");

        Id = id;
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreAircraftModel.Crear(nombre);
    }

    public void ActualizarEspecificaciones(
        int? autonomiKm,
        int? velocidadCruceroKmh)
    {
        AutonomiKm = autonomiKm is not null
            ? AutonomiKmAircraftModel.Crear(autonomiKm.Value)
            : null;

        VelocidadCruceroKmh = velocidadCruceroKmh is not null
            ? VelocidadCruceroKmhAircraftModel.Crear(velocidadCruceroKmh.Value)
            : null;
    }

    public void ActualizarDescripcion(string? descripcion)
    {
        Descripcion = descripcion is not null
            ? DescripcionAircraftModel.Crear(descripcion)
            : null;
    }

    // Propiedades de negocio
    public bool TieneEspecificacionesCompletas =>
        AutonomiKm is not null && VelocidadCruceroKmh is not null;

    public string AutonomiEnMillas =>
        AutonomiKm is not null
            ? $"{AutonomiKm.EnMillas} mi"
            : "No registrada";

    public string VelocidadEnNudos =>
        VelocidadCruceroKmh is not null
            ? $"{VelocidadCruceroKmh.EnNudos} kt"
            : "No registrada";

    public override string ToString() =>
        $"[{CodigoModelo}] {Nombre}";
}