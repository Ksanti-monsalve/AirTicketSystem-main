// src/modules/pilotlicense/Domain/ValueObjects/NumeroLicenciaPilotLicense.cs
namespace AirTicketSystem.modules.pilotlicense.Domain.ValueObjects;

public sealed class NumeroLicenciaPilotLicense
{
    public string Valor { get; }

    private NumeroLicenciaPilotLicense(string valor) => Valor = valor;

    public static NumeroLicenciaPilotLicense Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El número de licencia no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length < 5)
            throw new ArgumentException(
                "El número de licencia debe tener al menos 5 caracteres.");

        if (normalizado.Length > 50)
            throw new ArgumentException(
                "El número de licencia no puede superar 50 caracteres.");

        if (!normalizado.All(c => char.IsLetterOrDigit(c) || c == '-'))
            throw new ArgumentException(
                $"El número de licencia solo puede contener letras, números y guión. Se recibió: '{valor}'");

        return new NumeroLicenciaPilotLicense(normalizado);
    }

    public override string ToString() => Valor;
}