// src/modules/workertype/Domain/aggregate/WorkerType.cs
using AirTicketSystem.modules.workertype.Domain.ValueObjects;

namespace AirTicketSystem.modules.workertype.Domain.aggregate;

public sealed class WorkerType
{
    public int Id { get; private set; }
    public NombreWorkerType Nombre { get; private set; } = null!;

    private WorkerType() { }

    public static WorkerType Crear(string nombre)
    {
        return new WorkerType
        {
            Nombre = NombreWorkerType.Crear(nombre)
        };
    }

    public static WorkerType Reconstituir(int id, string nombre)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tipo de trabajador no es valido.");

        var workerType = Crear(nombre);
        workerType.Id = id;
        return workerType;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tipo de trabajador no es valido.");

        Id = id;
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreWorkerType.Crear(nombre);
    }

    public override string ToString() => Nombre.ToString();
}