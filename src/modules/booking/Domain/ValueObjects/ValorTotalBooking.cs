// src/modules/booking/Domain/ValueObjects/ValorTotalBooking.cs
namespace AirTicketSystem.modules.booking.Domain.ValueObjects;

public sealed class ValorTotalBooking
{
    public decimal Valor { get; }

    private ValorTotalBooking(decimal valor) => Valor = valor;

    public static ValorTotalBooking Crear(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentException("El valor total de la reserva no puede ser negativo.");

        if (valor == 0)
            throw new ArgumentException("El valor total de la reserva debe ser mayor a 0.");

        if (valor > 999_999_999)
            throw new ArgumentException(
                "El valor total de la reserva no puede superar 999.999.999.");

        return new ValorTotalBooking(Math.Round(valor, 2));
    }

    /// <summary>
    /// Calcula el valor total multiplicando el precio de la tarifa
    /// por la cantidad de pasajeros.
    /// </summary>
    public static ValorTotalBooking Calcular(decimal precioTarifa, int cantidadPasajeros)
    {
        if (precioTarifa <= 0)
            throw new ArgumentException("El precio de la tarifa debe ser mayor a 0.");

        if (cantidadPasajeros <= 0)
            throw new ArgumentException("La cantidad de pasajeros debe ser mayor a 0.");

        if (cantidadPasajeros > 9)
            throw new ArgumentException(
                "Una reserva no puede incluir más de 9 pasajeros.");

        return Crear(precioTarifa * cantidadPasajeros);
    }

    public override string ToString() => $"{Valor:N2}";
}