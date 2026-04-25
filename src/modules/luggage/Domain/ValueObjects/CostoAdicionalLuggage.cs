// src/modules/luggage/Domain/ValueObjects/CostoAdicionalLuggage.cs
namespace AirTicketSystem.modules.luggage.Domain.ValueObjects;

public sealed class CostoAdicionalLuggage
{
    public decimal Valor { get; }

    private CostoAdicionalLuggage(decimal valor) => Valor = valor;

    public static CostoAdicionalLuggage Crear(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentException(
                "El costo adicional del equipaje no puede ser negativo.");

        if (valor > 99_999_999)
            throw new ArgumentException(
                "El costo adicional no puede superar 99.999.999.");

        return new CostoAdicionalLuggage(Math.Round(valor, 2));
    }

    public static CostoAdicionalLuggage SinCosto() => new(0);

    public bool TieneCosto => Valor > 0;

    /// <summary>
    /// Calcula el costo adicional por exceso de peso.
    /// </summary>
    public static CostoAdicionalLuggage PorExcesoDePeso(
        decimal pesoReal,
        decimal pesoMaximo,
        decimal costoPorKgExcedido)
    {
        if (costoPorKgExcedido < 0)
            throw new ArgumentException(
                "El costo por kg excedido no puede ser negativo.");

        var excedente = pesoReal - pesoMaximo;

        if (excedente <= 0)
            return SinCosto();

        return Crear(Math.Round(excedente * costoPorKgExcedido, 2));
    }

    /// <summary>
    /// Calcula el costo adicional por exceso de dimensiones.
    /// Si alguna dimensión excede el límite, se aplica un cargo fijo.
    /// </summary>
    public static CostoAdicionalLuggage PorExcesoDeDimensiones(
        int largoReal, int anchoReal, int altoReal,
        int largoMax, int anchoMax, int altoMax,
        decimal cargoFijoPorDimension)
    {
        if (cargoFijoPorDimension < 0)
            throw new ArgumentException(
                "El cargo fijo por dimensión no puede ser negativo.");

        var cargos = 0m;

        if (largoReal > largoMax) cargos += cargoFijoPorDimension;
        if (anchoReal > anchoMax) cargos += cargoFijoPorDimension;
        if (altoReal > altoMax)   cargos += cargoFijoPorDimension;

        return Crear(cargos);
    }

    public override string ToString() => Valor == 0
        ? "Sin costo adicional"
        : $"{Valor:N2}";
}