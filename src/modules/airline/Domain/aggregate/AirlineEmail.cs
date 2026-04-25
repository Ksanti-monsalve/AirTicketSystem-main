// src/modules/airline/Domain/aggregate/AirlineEmail.cs
using AirTicketSystem.modules.airline.Domain.ValueObjects;

namespace AirTicketSystem.modules.airline.Domain.aggregate;

public sealed class AirlineEmail
{
    public int Id { get; private set; }
    public int AerolineaId { get; private set; }
    public int TipoEmailId { get; private set; }
    public EmailAirlineEmail Email { get; private set; } = null!;
    public EsPrincipalAirlineEmail EsPrincipal { get; private set; } = null!;

    private AirlineEmail() { }

    public static AirlineEmail Crear(
        int aerolineaId,
        int tipoEmailId,
        string email,
        bool esPrincipal = false)
    {
        if (aerolineaId <= 0)
            throw new ArgumentException("La aerolínea es obligatoria.");

        if (tipoEmailId <= 0)
            throw new ArgumentException("El tipo de email es obligatorio.");

        return new AirlineEmail
        {
            AerolineaId = aerolineaId,
            TipoEmailId = tipoEmailId,
            Email       = EmailAirlineEmail.Crear(email),
            EsPrincipal = EsPrincipalAirlineEmail.Crear(esPrincipal)
        };
    }

    public static AirlineEmail Reconstituir(
        int id, int aerolineaId, int tipoEmailId, string email, bool esPrincipal)
    {
        var ae = Crear(aerolineaId, tipoEmailId, email, esPrincipal);
        ae.Id = id;
        return ae;
    }

    public void EstablecerId(int id) => Id = id;

    public void ActualizarEmail(string email)
    {
        Email = EmailAirlineEmail.Crear(email);
    }

    public void MarcarComoPrincipal()
    {
        if (EsPrincipal.Valor)
            throw new InvalidOperationException(
                "Este email ya es el email principal.");

        EsPrincipal = EsPrincipalAirlineEmail.Principal();
    }

    public void MarcarComoSecundario()
    {
        if (!EsPrincipal.Valor)
            throw new InvalidOperationException(
                "Este email ya es un email secundario.");

        EsPrincipal = EsPrincipalAirlineEmail.Secundario();
    }

    public override string ToString() => Email.ToString();
}