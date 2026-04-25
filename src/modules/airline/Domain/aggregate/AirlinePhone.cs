// src/modules/airline/Domain/aggregate/AirlinePhone.cs
using AirTicketSystem.modules.airline.Domain.ValueObjects;

namespace AirTicketSystem.modules.airline.Domain.aggregate;

public sealed class AirlinePhone
{
    public int Id { get; private set; }
    public int AerolineaId { get; private set; }
    public int TipoTelefonoId { get; private set; }
    public NumeroAirlinePhone Numero { get; private set; } = null!;
    public IndicativoPaisAirlinePhone? IndicativoPais { get; private set; }
    public EsPrincipalAirlinePhone EsPrincipal { get; private set; } = null!;

    private AirlinePhone() { }

    public static AirlinePhone Crear(
        int aerolineaId,
        int tipoTelefonoId,
        string numero,
        string? indicativoPais = null,
        bool esPrincipal = false)
    {
        if (aerolineaId <= 0)
            throw new ArgumentException("La aerolínea es obligatoria.");

        if (tipoTelefonoId <= 0)
            throw new ArgumentException("El tipo de teléfono es obligatorio.");

        return new AirlinePhone
        {
            AerolineaId    = aerolineaId,
            TipoTelefonoId = tipoTelefonoId,
            Numero         = NumeroAirlinePhone.Crear(numero),
            IndicativoPais = indicativoPais is not null
                ? IndicativoPaisAirlinePhone.Crear(indicativoPais)
                : null,
            EsPrincipal = EsPrincipalAirlinePhone.Crear(esPrincipal)
        };
    }

    public static AirlinePhone Reconstituir(
        int id, int aerolineaId, int tipoTelefonoId,
        string numero, string? indicativoPais, bool esPrincipal)
    {
        var ap = Crear(aerolineaId, tipoTelefonoId, numero, indicativoPais, esPrincipal);
        ap.Id = id;
        return ap;
    }

    public void EstablecerId(int id) => Id = id;

    public void ActualizarNumero(string numero, string? indicativoPais = null)
    {
        Numero = NumeroAirlinePhone.Crear(numero);
        IndicativoPais = indicativoPais is not null
            ? IndicativoPaisAirlinePhone.Crear(indicativoPais)
            : null;
    }

    public void MarcarComoPrincipal()
    {
        if (EsPrincipal.Valor)
            throw new InvalidOperationException(
                "Este teléfono ya es el teléfono principal.");

        EsPrincipal = EsPrincipalAirlinePhone.Principal();
    }

    public void MarcarComoSecundario()
    {
        if (!EsPrincipal.Valor)
            throw new InvalidOperationException(
                "Este teléfono ya es un teléfono secundario.");

        EsPrincipal = EsPrincipalAirlinePhone.Secundario();
    }

    public string NumeroCompleto =>
        IndicativoPais is not null
            ? $"{IndicativoPais} {Numero}"
            : Numero.ToString();

    public override string ToString() => NumeroCompleto;
}