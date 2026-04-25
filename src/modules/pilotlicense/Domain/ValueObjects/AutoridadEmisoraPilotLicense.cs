// src/modules/pilotlicense/Domain/ValueObjects/AutoridadEmisoraPilotLicense.cs
namespace AirTicketSystem.modules.pilotlicense.Domain.ValueObjects;

public sealed class AutoridadEmisoraPilotLicense
{
    public string Valor { get; }

    private AutoridadEmisoraPilotLicense(string valor) => Valor = valor;

    public static AutoridadEmisoraPilotLicense Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("La autoridad emisora no puede estar vacía.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 3)
            throw new ArgumentException(
                "El nombre de la autoridad emisora debe tener al menos 3 caracteres.");

        if (normalizado.Length > 100)
            throw new ArgumentException(
                "El nombre de la autoridad emisora no puede superar 100 caracteres.");

        return new AutoridadEmisoraPilotLicense(normalizado);
    }

    public override string ToString() => Valor;
}