// src/modules/gate/Domain/aggregate/Gate.cs
using AirTicketSystem.modules.gate.Domain.ValueObjects;

namespace AirTicketSystem.modules.gate.Domain.aggregate;

public sealed class Gate
{
    public int Id { get; private set; }
    public int TerminalId { get; private set; }
    public CodigoGate Codigo { get; private set; } = null!;
    public ActivaGate Activa { get; private set; } = null!;

    private Gate() { }

    public static Gate Crear(int terminalId, string codigo)
    {
        if (terminalId <= 0)
            throw new ArgumentException("La terminal es obligatoria.");

        return new Gate
        {
            TerminalId = terminalId,
            Codigo     = CodigoGate.Crear(codigo),
            Activa     = ActivaGate.Activa()
        };
    }

    public static Gate Reconstituir(
        int id,
        int terminalId,
        string codigo,
        bool activa)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la puerta no es valido.");

        var gate = Crear(terminalId, codigo);
        gate.Id = id;
        gate.Activa = ActivaGate.Crear(activa);
        return gate;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la puerta no es valido.");

        Id = id;
    }

    public void ActualizarCodigo(string codigo)
    {
        Codigo = CodigoGate.Crear(codigo);
    }

    public void Activar() => Activa = ActivaGate.Activa();

    public void Desactivar() => Activa = ActivaGate.Inactiva();

    public override string ToString() => $"Puerta {Codigo}";
}