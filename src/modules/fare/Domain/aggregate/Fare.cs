// src/modules/fare/Domain/aggregate/Fare.cs
using AirTicketSystem.modules.fare.Domain.ValueObjects;

namespace AirTicketSystem.modules.fare.Domain.aggregate;

public sealed class Fare
{
    public int Id { get; private set; }
    public int RutaId { get; private set; }
    public int ClaseServicioId { get; private set; }
    public NombreFare Nombre { get; private set; } = null!;
    public PrecioBaseFare PrecioBase { get; private set; } = null!;
    public ImpuestosFare Impuestos { get; private set; } = null!;
    public PrecioTotalFare PrecioTotal { get; private set; } = null!;
    public PermiteCambiosFare PermiteCambios { get; private set; } = null!;
    public PermiteReembolsoFare PermiteReembolso { get; private set; } = null!;
    public ActivaFare Activa { get; private set; } = null!;
    public VigenteHastaFare? VigenteHasta { get; private set; }

    private Fare() { }

    public static Fare Reconstituir(
        int id,
        int rutaId,
        int claseServicioId,
        string nombre,
        decimal precioBase,
        decimal impuestos,
        decimal precioTotal,
        bool permiteCambios,
        bool permiteReembolso,
        bool activa,
        DateOnly? vigenteHasta)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la tarifa no es válido.");

        return new Fare
        {
            Id               = id,
            RutaId           = rutaId,
            ClaseServicioId  = claseServicioId,
            Nombre           = NombreFare.Crear(nombre),
            PrecioBase       = PrecioBaseFare.Crear(precioBase),
            Impuestos        = ImpuestosFare.Crear(impuestos),
            PrecioTotal      = PrecioTotalFare.Crear(precioTotal),
            PermiteCambios   = PermiteCambiosFare.Crear(permiteCambios),
            PermiteReembolso = PermiteReembolsoFare.Crear(permiteReembolso),
            Activa           = ActivaFare.Crear(activa),
            VigenteHasta     = vigenteHasta is not null
                ? VigenteHastaFare.Crear(vigenteHasta.Value)
                : null
        };
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la tarifa no es válido.");

        Id = id;
    }

    public static Fare Crear(
        int rutaId,
        int claseServicioId,
        string nombre,
        decimal precioBase,
        decimal impuestos,
        bool permiteCambios = false,
        bool permiteReembolso = false,
        DateOnly? vigenteHasta = null)
    {
        if (rutaId <= 0)
            throw new ArgumentException("La ruta es obligatoria.");

        if (claseServicioId <= 0)
            throw new ArgumentException("La clase de servicio es obligatoria.");

        var base_     = PrecioBaseFare.Crear(precioBase);
        var impuest   = ImpuestosFare.Crear(impuestos);
        var total     = PrecioTotalFare.Calcular(precioBase, impuestos);

        return new Fare
        {
            RutaId           = rutaId,
            ClaseServicioId  = claseServicioId,
            Nombre           = NombreFare.Crear(nombre),
            PrecioBase       = base_,
            Impuestos        = impuest,
            PrecioTotal      = total,
            PermiteCambios   = PermiteCambiosFare.Crear(permiteCambios),
            PermiteReembolso = PermiteReembolsoFare.Crear(permiteReembolso),
            Activa           = ActivaFare.Activa(),
            VigenteHasta     = vigenteHasta is not null
                ? VigenteHastaFare.Crear(vigenteHasta.Value)
                : null
        };
    }

    // ── Gestión de precios ───────────────────────────────────

    public void ActualizarPrecios(decimal precioBase, decimal impuestos)
    {
        if (!Activa.Valor)
            throw new InvalidOperationException(
                "No se pueden actualizar los precios de una tarifa inactiva.");

        PrecioBase  = PrecioBaseFare.Crear(precioBase);
        Impuestos   = ImpuestosFare.Crear(impuestos);
        PrecioTotal = PrecioTotalFare.Calcular(precioBase, impuestos);
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreFare.Crear(nombre);
    }

    public void ActualizarPoliticas(bool permiteCambios, bool permiteReembolso)
    {
        PermiteCambios   = PermiteCambiosFare.Crear(permiteCambios);
        PermiteReembolso = PermiteReembolsoFare.Crear(permiteReembolso);
    }

    public void ActualizarVigencia(DateOnly? vigenteHasta)
    {
        VigenteHasta = vigenteHasta is not null
            ? VigenteHastaFare.Crear(vigenteHasta.Value)
            : null;
    }

    public void Activar()
    {
        if (Activa.Valor)
            throw new InvalidOperationException(
                "La tarifa ya se encuentra activa.");

        Activa = ActivaFare.Activa();
    }

    public void Desactivar()
    {
        if (!Activa.Valor)
            throw new InvalidOperationException(
                "La tarifa ya se encuentra inactiva.");

        Activa = ActivaFare.Inactiva();
    }

    // ── Propiedades de negocio ───────────────────────────────

    public bool EstaDisponible =>
        Activa.Valor &&
        (VigenteHasta is null || VigenteHasta.EstaVigente);

    public bool VenceProximamente =>
        VigenteHasta is not null && VigenteHasta.VenceProximamente;

    public bool EsFlexible =>
        PermiteCambios.Valor && PermiteReembolso.Valor;

    public bool EsRestrictiva =>
        !PermiteCambios.Valor && !PermiteReembolso.Valor;

    public int? DiasDeVigencia =>
        VigenteHasta?.DiasDeVigencia;

    /// <summary>
    /// Calcula el valor total para una cantidad de pasajeros.
    /// </summary>
    public decimal CalcularValorParaPasajeros(int cantidadPasajeros)
    {
        if (cantidadPasajeros <= 0)
            throw new ArgumentException(
                "La cantidad de pasajeros debe ser mayor a 0.");

        if (cantidadPasajeros > 9)
            throw new ArgumentException(
                "Una reserva no puede incluir más de 9 pasajeros.");

        if (!EstaDisponible)
            throw new InvalidOperationException(
                "No se puede calcular el valor de una tarifa no disponible.");

        return Math.Round(PrecioTotal.Valor * cantidadPasajeros, 2);
    }

    public override string ToString() =>
        $"[{Nombre}] {PrecioTotal} — " +
        $"{(EstaDisponible ? "Disponible" : "No disponible")}";
}