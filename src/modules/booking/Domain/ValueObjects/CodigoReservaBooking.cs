// src/modules/booking/Domain/ValueObjects/CodigoReservaBooking.cs
namespace AirTicketSystem.modules.booking.Domain.ValueObjects;

public sealed class CodigoReservaBooking
{
    public string Valor { get; }

    private CodigoReservaBooking(string valor) => Valor = valor;

    public static CodigoReservaBooking Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código de reserva no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length != 6)
            throw new ArgumentException(
                $"El código de reserva debe tener exactamente 6 caracteres. Se recibió: '{valor}'");

        if (!normalizado.All(char.IsLetterOrDigit))
            throw new ArgumentException(
                $"El código de reserva solo puede contener letras y números. Se recibió: '{valor}'");

        return new CodigoReservaBooking(normalizado);
    }

    /// <summary>
    /// Genera un PNR aleatorio de 6 caracteres alfanuméricos.
    /// Se excluyen caracteres ambiguos: O, 0, I, 1 para evitar
    /// confusiones al leerlo en pantalla o en papel.
    /// </summary>
    public static CodigoReservaBooking Generar()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        var random = new Random();
        var codigo = new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        return new CodigoReservaBooking(codigo);
    }

    public override string ToString() => Valor;
}