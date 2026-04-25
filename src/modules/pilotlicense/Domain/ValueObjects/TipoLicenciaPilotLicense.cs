// src/modules/pilotlicense/Domain/ValueObjects/TipoLicenciaPilotLicense.cs
namespace AirTicketSystem.modules.pilotlicense.Domain.ValueObjects;

public sealed class TipoLicenciaPilotLicense
{
    private static readonly string[] _tiposValidos = { "PPL", "CPL", "ATPL" };

    public string Valor { get; }

    private TipoLicenciaPilotLicense(string valor) => Valor = valor;

    public static TipoLicenciaPilotLicense Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El tipo de licencia no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!_tiposValidos.Contains(normalizado))
            throw new ArgumentException(
                $"El tipo de licencia '{valor}' no es válido. " +
                $"Los valores permitidos son: {string.Join(", ", _tiposValidos)}");

        return new TipoLicenciaPilotLicense(normalizado);
    }

    // Métodos de fábrica expresivos
    public static TipoLicenciaPilotLicense Ppl() => new("PPL");
    public static TipoLicenciaPilotLicense Cpl() => new("CPL");
    public static TipoLicenciaPilotLicense Atpl() => new("ATPL");

    // Propiedades de negocio
    public bool EsPrivada => Valor == "PPL";
    public bool EsComercial => Valor == "CPL";
    public bool EsLineaAerea => Valor == "ATPL";

    // Un piloto comercial puede operar vuelos de pasajeros
    public bool HabilitaVuelosComerciales => Valor == "CPL" || Valor == "ATPL";

    public string Descripcion => Valor switch
    {
        "PPL"  => "Licencia de Piloto Privado",
        "CPL"  => "Licencia de Piloto Comercial",
        "ATPL" => "Licencia de Piloto de Línea Aérea",
        _      => Valor
    };

    public override string ToString() => $"{Valor} — {Descripcion}";
}