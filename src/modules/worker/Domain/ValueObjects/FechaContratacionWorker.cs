// src/modules/worker/Domain/ValueObjects/FechaContratacionWorker.cs
namespace AirTicketSystem.modules.worker.Domain.ValueObjects;

public sealed class FechaContratacionWorker
{
    public DateOnly Valor { get; }

    private FechaContratacionWorker(DateOnly valor) => Valor = valor;

    public static FechaContratacionWorker Crear(DateOnly valor)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);

        if (valor > hoy)
            throw new ArgumentException(
                "La fecha de contratación no puede ser una fecha futura.");

        if (valor < new DateOnly(1950, 1, 1))
            throw new ArgumentException(
                "La fecha de contratación no puede ser anterior a 1950.");

        return new FechaContratacionWorker(valor);
    }

    public int AnosEnLaEmpresa
    {
        get
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var anos = hoy.Year - Valor.Year;
            if (Valor > hoy.AddYears(-anos)) anos--;
            return anos;
        }
    }

    public bool EsEmpleadoNuevo => AnosEnLaEmpresa < 1;

    public override string ToString() => Valor.ToString("dd/MM/yyyy");
}