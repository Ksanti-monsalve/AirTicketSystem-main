// src/modules/luggagerestriction/Domain/ValueObjects/CostoExcesoKgLuggageRestriction.cs
namespace AirTicketSystem.modules.luggagerestriction.Domain.ValueObjects;

public sealed class CostoExcesoKgLuggageRestriction
{
    public decimal Valor { get; }

    private CostoExcesoKgLuggageRestriction(decimal valor) => Valor = valor;

    public static CostoExcesoKgLuggageRestriction Crear(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentException(
                "El costo por exceso de kg no puede ser negativo.");

        if (valor > 999_999)
            throw new ArgumentException(
                "El costo por exceso de kg no puede superar 999.999.");

        return new CostoExcesoKgLuggageRestriction(Math.Round(valor, 2));
    }

    public static CostoExcesoKgLuggageRestriction SinCosto() => new(0);

    public bool TieneCosto => Valor > 0;

    /// <summary>
    /// Calcula el costo total por los kg excedidos.
    /// </summary>
    public decimal CalcularCostoExcedente(decimal kgExcedidos)
    {
        if (kgExcedidos <= 0) return 0;
        return Math.Round(Valor * kgExcedidos, 2);
    }

    public override string ToString() => Valor == 0
        ? "Sin costo por exceso"
        : $"{Valor:N2} por kg excedido";
}