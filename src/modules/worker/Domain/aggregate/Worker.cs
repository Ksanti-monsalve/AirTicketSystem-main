// src/modules/worker/Domain/aggregate/Worker.cs
using AirTicketSystem.modules.worker.Domain.ValueObjects;

namespace AirTicketSystem.modules.worker.Domain.aggregate;

public sealed class Worker
{
    public int Id { get; private set; }
    public int PersonaId { get; private set; }
    public int TipoTrabajadorId { get; private set; }
    public int? AerolineaId { get; private set; }
    public int AeropuertoBaseId { get; private set; }
    public int? UsuarioId { get; private set; }
    public FechaContratacionWorker FechaContratacion { get; private set; } = null!;
    public SalarioWorker Salario { get; private set; } = null!;
    public ActivoWorker Activo { get; private set; } = null!;

    private Worker() { }

    public static Worker Crear(
        int personaId,
        int tipoTrabajadorId,
        int aeropuertoBaseId,
        DateOnly fechaContratacion,
        decimal salario,
        int? aerolineaId = null,
        int? usuarioId = null)
    {
        if (personaId <= 0)
            throw new ArgumentException("La persona es obligatoria.");

        if (tipoTrabajadorId <= 0)
            throw new ArgumentException("El tipo de trabajador es obligatorio.");

        if (aeropuertoBaseId <= 0)
            throw new ArgumentException("El aeropuerto base es obligatorio.");

        return new Worker
        {
            PersonaId         = personaId,
            TipoTrabajadorId  = tipoTrabajadorId,
            AeropuertoBaseId  = aeropuertoBaseId,
            AerolineaId       = aerolineaId,
            UsuarioId         = usuarioId,
            FechaContratacion = FechaContratacionWorker.Crear(fechaContratacion),
            Salario           = SalarioWorker.Crear(salario),
            Activo            = ActivoWorker.Activo()
        };
    }

    public static Worker Reconstituir(
        int id,
        int personaId,
        int tipoTrabajadorId,
        int aeropuertoBaseId,
        DateOnly fechaContratacion,
        decimal salario,
        bool activo,
        int? aerolineaId,
        int? usuarioId)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del trabajador no es valido.");

        var worker = Crear(
            personaId,
            tipoTrabajadorId,
            aeropuertoBaseId,
            fechaContratacion,
            salario,
            aerolineaId,
            usuarioId);

        worker.Id = id;
        worker.Activo = ActivoWorker.Crear(activo);
        return worker;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del trabajador no es valido.");

        Id = id;
    }

    // ── Gestión de trabajador ────────────────────────────────

    public void ActualizarSalario(decimal nuevoSalario)
    {
        if (nuevoSalario <= Salario.Valor)
            throw new InvalidOperationException(
                "El nuevo salario debe ser mayor al salario actual. " +
                "Para reducciones salariales contacte al área de RRHH.");

        Salario = SalarioWorker.Crear(nuevoSalario);
    }

    public void ActualizarAeropuertoBase(int aeropuertoBaseId)
    {
        if (aeropuertoBaseId <= 0)
            throw new ArgumentException("El aeropuerto base es obligatorio.");

        if (aeropuertoBaseId == AeropuertoBaseId)
            throw new InvalidOperationException(
                "El aeropuerto base indicado es el mismo que el actual.");

        AeropuertoBaseId = aeropuertoBaseId;
    }

    public void AsignarAerolinea(int? aerolineaId)
    {
        if (aerolineaId.HasValue && aerolineaId <= 0)
            throw new ArgumentException("La aerolínea no es válida.");

        AerolineaId = aerolineaId;
    }

    public void AsignarUsuario(int usuarioId)
    {
        if (usuarioId <= 0)
            throw new ArgumentException("El usuario no es válido.");

        if (UsuarioId.HasValue)
            throw new InvalidOperationException(
                "El trabajador ya tiene un usuario asignado.");

        UsuarioId = usuarioId;
    }

    public void Activar()
    {
        if (Activo.Valor)
            throw new InvalidOperationException(
                "El trabajador ya se encuentra activo.");

        Activo = ActivoWorker.Activo();
    }

    public void Desactivar()
    {
        if (!Activo.Valor)
            throw new InvalidOperationException(
                "El trabajador ya se encuentra inactivo.");

        Activo = ActivoWorker.Inactivo();
    }

    // ── Propiedades de negocio ───────────────────────────────

    public bool EstaActivo => Activo.Valor;

    public bool EsEmpleadoNuevo =>
        FechaContratacion.EsEmpleadoNuevo;

    public int AnosEnLaEmpresa =>
        FechaContratacion.AnosEnLaEmpresa;

    public decimal SalarioAnual =>
        Salario.SalarioAnual;

    public bool TieneUsuarioAsignado =>
        UsuarioId.HasValue;

    public bool PerteneceAUnaAerolinea =>
        AerolineaId.HasValue;

    public override string ToString() =>
        $"Trabajador #{Id} — Persona {PersonaId}";
}