// src/modules/person/Domain/aggregate/PersonPhone.cs
using AirTicketSystem.modules.person.Domain.ValueObjects;

namespace AirTicketSystem.modules.person.Domain.aggregate;

public sealed class PersonPhone
{
    public int Id { get; private set; }
    public int PersonaId { get; private set; }
    public int TipoTelefonoId { get; private set; }
    public NumeroPersonPhone Numero { get; private set; } = null!;
    public IndicativoPaisPersonPhone? IndicativoPais { get; private set; }
    public EsPrincipalPersonPhone EsPrincipal { get; private set; } = null!;

    private PersonPhone() { }

    public static PersonPhone Crear(
        int personaId,
        int tipoTelefonoId,
        string numero,
        string? indicativoPais = null,
        bool esPrincipal = false)
    {
        if (personaId <= 0)
            throw new ArgumentException("La persona es obligatoria.");

        if (tipoTelefonoId <= 0)
            throw new ArgumentException("El tipo de teléfono es obligatorio.");

        return new PersonPhone
        {
            PersonaId      = personaId,
            TipoTelefonoId = tipoTelefonoId,
            Numero         = NumeroPersonPhone.Crear(numero),
            IndicativoPais = indicativoPais is not null
                ? IndicativoPaisPersonPhone.Crear(indicativoPais)
                : null,
            EsPrincipal = EsPrincipalPersonPhone.Crear(esPrincipal)
        };
    }

    public static PersonPhone Reconstituir(
        int id,
        int personaId,
        int tipoTelefonoId,
        string numero,
        string? indicativoPais,
        bool esPrincipal)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del teléfono no es válido.");

        var phone = Crear(personaId, tipoTelefonoId, numero, indicativoPais, esPrincipal);
        phone.Id = id;
        return phone;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del teléfono no es válido.");

        Id = id;
    }

    public void MarcarComoPrincipal()
    {
        EsPrincipal = EsPrincipalPersonPhone.Principal();
    }

    public void MarcarComoSecundario()
    {
        EsPrincipal = EsPrincipalPersonPhone.Secundario();
    }

    public void ActualizarNumero(string numero, string? indicativoPais = null)
    {
        Numero         = NumeroPersonPhone.Crear(numero);
        IndicativoPais = indicativoPais is not null
            ? IndicativoPaisPersonPhone.Crear(indicativoPais)
            : null;
    }

    public string NumeroCompleto =>
        IndicativoPais is not null
            ? $"{IndicativoPais} {Numero}"
            : Numero.ToString();

    public override string ToString() => NumeroCompleto;
}