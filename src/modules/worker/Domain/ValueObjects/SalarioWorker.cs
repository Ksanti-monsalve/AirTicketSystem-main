// src/modules/worker/Domain/ValueObjects/SalarioWorker.cs
namespace AirTicketSystem.modules.worker.Domain.ValueObjects;

public sealed class SalarioWorker
{
    public decimal Valor { get; }

    private SalarioWorker(decimal valor) => Valor = valor;

    public static SalarioWorker Crear(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentException("El salario no puede ser negativo.");

        // Salario mínimo referencial — ajustable según país
        if (valor < 500_000)
            throw new ArgumentException(
                "El salario no puede ser inferior a 500.000.");

        if (valor > 100_000_000)
            throw new ArgumentException(
                "El salario no puede superar 100.000.000.");

        return new SalarioWorker(Math.Round(valor, 2));
    }

    public decimal SalarioMensual => Valor;
    public decimal SalarioAnual => Valor * 12;

    public override string ToString() => $"{Valor:N2}";
}