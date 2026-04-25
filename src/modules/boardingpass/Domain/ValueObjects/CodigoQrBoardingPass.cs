// src/modules/boardingpass/Domain/ValueObjects/CodigoQrBoardingPass.cs
namespace AirTicketSystem.modules.boardingpass.Domain.ValueObjects;

public sealed class CodigoQrBoardingPass
{
    public string Valor { get; }

    private CodigoQrBoardingPass(string valor) => Valor = valor;

    public static CodigoQrBoardingPass Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código QR no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 10)
            throw new ArgumentException(
                "El contenido del código QR debe tener al menos 10 caracteres.");

        if (normalizado.Length > 500)
            throw new ArgumentException(
                "El contenido del código QR no puede superar 500 caracteres.");

        return new CodigoQrBoardingPass(normalizado);
    }

    /// <summary>
    /// Genera el contenido del QR en formato estándar BCBP
    /// (Bar Coded Boarding Pass) simplificado para uso académico.
    /// Formato: BP|[codigoPase]|[numeroVuelo]|[asiento]|[fecha]
    /// </summary>
    public static CodigoQrBoardingPass Generar(
        string codigoPase,
        string numeroVuelo,
        string codigoAsiento,
        DateTime fechaSalida)
    {
        if (string.IsNullOrWhiteSpace(codigoPase))
            throw new ArgumentException("El código del pase no puede estar vacío.");

        if (string.IsNullOrWhiteSpace(numeroVuelo))
            throw new ArgumentException("El número de vuelo no puede estar vacío.");

        if (string.IsNullOrWhiteSpace(codigoAsiento))
            throw new ArgumentException("El código de asiento no puede estar vacío.");

        var contenido =
            $"BP|{codigoPase.ToUpperInvariant()}" +
            $"|{numeroVuelo.ToUpperInvariant()}" +
            $"|{codigoAsiento.ToUpperInvariant()}" +
            $"|{fechaSalida:yyyyMMddHHmm}";

        return new CodigoQrBoardingPass(contenido);
    }

    public override string ToString() => Valor;
}