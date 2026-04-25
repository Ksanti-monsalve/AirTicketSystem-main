// src/modules/aircraft/Domain/aggregate/Aircraft.cs
using AirTicketSystem.modules.aircraft.Domain.ValueObjects;

namespace AirTicketSystem.modules.aircraft.Domain.aggregate;

public sealed class Aircraft
{
    public int Id { get; private set; }
    public int ModeloAvionId { get; private set; }
    public int AerolineaId { get; private set; }
    public MatriculaAircraft Matricula { get; private set; } = null!;
    public FechaFabricacionAircraft? FechaFabricacion { get; private set; }
    public FechaUltimoMantenimientoAircraft? FechaUltimoMantenimiento { get; private set; }
    public FechaProximoMantenimientoAircraft? FechaProximoMantenimiento { get; private set; }
    public TotalHorasVueloAircraft TotalHorasVuelo { get; private set; } = null!;
    public EstadoAircraft Estado { get; private set; } = null!;
    public ActivoAircraft Activo { get; private set; } = null!;

    private Aircraft() { }

    public static Aircraft Crear(
        int modeloAvionId,
        int aerolineaId,
        string matricula,
        DateOnly? fechaFabricacion = null,
        DateOnly? fechaProximoMantenimiento = null)
    {
        if (modeloAvionId <= 0)
            throw new ArgumentException("El modelo de avión es obligatorio.");

        if (aerolineaId <= 0)
            throw new ArgumentException("La aerolínea es obligatoria.");

        return new Aircraft
        {
            ModeloAvionId            = modeloAvionId,
            AerolineaId              = aerolineaId,
            Matricula                = MatriculaAircraft.Crear(matricula),
            FechaFabricacion         = fechaFabricacion is not null
                ? FechaFabricacionAircraft.Crear(fechaFabricacion.Value)
                : null,
            FechaUltimoMantenimiento = null,
            FechaProximoMantenimiento = fechaProximoMantenimiento is not null
                ? FechaProximoMantenimientoAircraft.Crear(fechaProximoMantenimiento.Value)
                : null,
            TotalHorasVuelo = TotalHorasVueloAircraft.Cero(),
            Estado          = EstadoAircraft.Disponible(),
            Activo          = ActivoAircraft.Activo()
        };
    }

    public static Aircraft Reconstituir(
        int id,
        int modeloAvionId,
        int aerolineaId,
        string matricula,
        DateOnly? fechaFabricacion,
        DateOnly? fechaUltimoMantenimiento,
        DateOnly? fechaProximoMantenimiento,
        decimal totalHorasVuelo,
        string estado,
        bool activo)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del avion no es valido.");

        var aircraft = Crear(
            modeloAvionId,
            aerolineaId,
            matricula,
            fechaFabricacion,
            fechaProximoMantenimiento);

        aircraft.Id = id;
        aircraft.FechaUltimoMantenimiento = fechaUltimoMantenimiento is not null
            ? FechaUltimoMantenimientoAircraft.Crear(fechaUltimoMantenimiento.Value)
            : null;
        aircraft.TotalHorasVuelo = TotalHorasVueloAircraft.Crear(totalHorasVuelo);
        aircraft.Estado = EstadoAircraft.Crear(estado);
        aircraft.Activo = ActivoAircraft.Crear(activo);
        return aircraft;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del avion no es valido.");

        Id = id;
    }

    public void ActualizarFechas(
        DateOnly? fechaFabricacion,
        DateOnly? fechaProximoMantenimiento)
    {
        FechaFabricacion = fechaFabricacion is not null
            ? FechaFabricacionAircraft.Crear(fechaFabricacion.Value)
            : null;

        FechaProximoMantenimiento = fechaProximoMantenimiento is not null
            ? FechaProximoMantenimientoAircraft.Crear(fechaProximoMantenimiento.Value)
            : null;
    }

    // ── Gestión de estado ────────────────────────────────────

    public void AsignarAVuelo()
    {
        if (!Estado.PuedeAsignarse)
            throw new InvalidOperationException(
                $"El avión no puede asignarse a un vuelo en estado '{Estado}'.");

        Estado = EstadoAircraft.EnVuelo();
    }

    public void RegistrarAterrizaje(decimal horasVuelo)
    {
        if (Estado.Valor != "EN_VUELO")
            throw new InvalidOperationException(
                "Solo se puede registrar aterrizaje de un avión en vuelo.");

        if (horasVuelo <= 0)
            throw new ArgumentException(
                "Las horas de vuelo deben ser mayores a 0.");

        TotalHorasVuelo = TotalHorasVuelo.Sumar(horasVuelo);
        Estado          = EstadoAircraft.Disponible();
    }

    public void EnviarAMantenimiento(DateOnly fechaProximoMantenimiento)
    {
        if (!Estado.EstaOperativo)
            throw new InvalidOperationException(
                "Solo se puede enviar a mantenimiento un avión operativo.");

        Estado = EstadoAircraft.EnMantenimiento();
        FechaProximoMantenimiento =
            FechaProximoMantenimientoAircraft.Crear(fechaProximoMantenimiento);
    }

    public void RegistrarMantenimiento(DateOnly fechaProximoMantenimiento)
    {
        if (Estado.Valor != "MANTENIMIENTO")
            throw new InvalidOperationException(
                "Solo se puede registrar mantenimiento de un avión en mantenimiento.");

        var hoy = DateOnly.FromDateTime(DateTime.Today);
        FechaUltimoMantenimiento  = FechaUltimoMantenimientoAircraft.Crear(hoy);
        FechaProximoMantenimiento = FechaProximoMantenimientoAircraft.Crear(fechaProximoMantenimiento);
        Estado                    = EstadoAircraft.Disponible();
    }

    public void DarDeBaja()
    {
        if (Estado.Valor == "EN_VUELO")
            throw new InvalidOperationException(
                "No se puede dar de baja un avión que está en vuelo.");

        Estado = EstadoAircraft.FueraDeServicio();
        Activo = ActivoAircraft.Inactivo();
    }

    public void Reactivar()
    {
        if (Activo.Valor)
            throw new InvalidOperationException(
                "El avión ya se encuentra activo.");

        Estado = EstadoAircraft.Disponible();
        Activo = ActivoAircraft.Activo();
    }

    // ── Propiedades de negocio ───────────────────────────────

    public bool RequiereMantenimientoUrgente =>
        FechaProximoMantenimiento is not null &&
        FechaProximoMantenimiento.EsUrgente;

    public bool EstaDisponible => Estado.PuedeAsignarse;

    public int? DiasDesdeUltimoMantenimiento =>
        FechaUltimoMantenimiento?.DiasDesdeUltimoMantenimiento;

    public int? AnosEnServicio =>
        FechaFabricacion?.AnosEnServicio;

    public override string ToString() =>
        $"[{Matricula}] — {Estado}";
}