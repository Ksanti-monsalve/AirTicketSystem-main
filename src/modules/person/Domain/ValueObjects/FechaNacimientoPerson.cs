// src/modules/person/Domain/ValueObjects/FechaNacimientoPerson.cs
namespace AirTicketSystem.modules.person.Domain.ValueObjects;

public sealed class FechaNacimientoPerson
{
    public DateOnly Valor { get; }

    private FechaNacimientoPerson(DateOnly valor) => Valor = valor;

    public static FechaNacimientoPerson Crear(DateOnly valor)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);

        if (valor > hoy)
            throw new ArgumentException("La fecha de nacimiento no puede ser una fecha futura.");

        if (valor < new DateOnly(1900, 1, 1))
            throw new ArgumentException("La fecha de nacimiento no puede ser anterior a 1900.");

        if (hoy.Year - valor.Year > 120)
            throw new ArgumentException("La fecha de nacimiento ingresada no es válida.");

        return new FechaNacimientoPerson(valor);
    }

    // Utilidades de negocio que pertenecen a este concepto
    public int Edad
    {
        get
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var edad = hoy.Year - Valor.Year;
            if (Valor > hoy.AddYears(-edad)) edad--;
            return edad;
        }
    }

    public bool EsMenorDeEdad => Edad < 18;
    public bool EsInfante => Edad < 2;

    public override string ToString() => Valor.ToString("dd/MM/yyyy");
}