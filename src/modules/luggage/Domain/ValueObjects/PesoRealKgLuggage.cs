// src/modules/luggage/Domain/ValueObjects/PesoRealKgLuggage.cs
namespace AirTicketSystem.modules.luggage.Domain.ValueObjects;

public sealed class PesoRealKgLuggage
{
    public decimal Valor { get; }

    private PesoRealKgLuggage(decimal valor) => Valor = valor;

    public static PesoRealKgLuggage Crear(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException(
                "El peso real debe ser mayor a 0 kg.");

        if (valor > 50)
            throw new ArgumentException(
                "El peso real no puede superar 50 kg por pieza.");

        return new PesoRealKgLuggage(Math.Round(valor, 2));
    }

    /// <summary>
    /// Compara el peso real contra el peso declarado.
    /// Útil para detectar subdeclaraciones significativas.
    /// </summary>
    public bool ExcedePesoDeclarado(decimal pesoDeclarado)
        => Valor > pesoDeclarado;

    public decimal DiferenciaConDeclarado(decimal pesoDeclarado)
        => Math.Round(Math.Abs(Valor - pesoDeclarado), 2);

    /// <summary>
    /// Calcula el excedente sobre el límite máximo permitido.
    /// Retorna 0 si no hay excedente.
    /// </summary>
    public decimal ExcedenteContra(decimal pesoMaximo)
    {
        var excedente = Valor - pesoMaximo;
        return excedente > 0 ? Math.Round(excedente, 2) : 0;
    }

    public override string ToString() => $"{Valor} kg";
}