// src/modules/worker/Domain/ValueObjects/ActivoWorker.cs
namespace AirTicketSystem.modules.worker.Domain.ValueObjects;

public sealed class ActivoWorker
{
    public bool Valor { get; }

    private ActivoWorker(bool valor) => Valor = valor;

    public static ActivoWorker Crear(bool valor) => new(valor);

    public static ActivoWorker Activo() => new(true);

    public static ActivoWorker Inactivo() => new(false);

    public override string ToString() => Valor ? "Activo" : "Inactivo";
}