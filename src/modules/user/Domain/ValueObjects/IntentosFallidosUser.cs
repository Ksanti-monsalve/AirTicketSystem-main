// src/modules/user/Domain/ValueObjects/IntentosFallidosUser.cs
namespace AirTicketSystem.modules.user.Domain.ValueObjects;

public sealed class IntentosFallidosUser
{
    public int Valor { get; }

    // Máximo de intentos permitidos antes de bloquear la cuenta
    public const int MaximoIntentos = 5;

    private IntentosFallidosUser(int valor) => Valor = valor;

    public static IntentosFallidosUser Crear(int valor)
    {
        if (valor < 0)
            throw new ArgumentException("Los intentos fallidos no pueden ser negativos.");

        if (valor > 100)
            throw new ArgumentException("El número de intentos fallidos excede el límite permitido.");

        return new IntentosFallidosUser(valor);
    }

    public static IntentosFallidosUser Cero() => new(0);

    public bool CuentaBloqueada => Valor >= MaximoIntentos;

    public IntentosFallidosUser Incrementar()
    {
        if (Valor >= MaximoIntentos)
            throw new InvalidOperationException(
                "La cuenta ya está bloqueada por exceso de intentos fallidos.");
        return new IntentosFallidosUser(Valor + 1);
    }

    public IntentosFallidosUser Reiniciar() => Cero();

    public override string ToString() => Valor.ToString();
}