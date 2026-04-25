// src/modules/luggage/Domain/aggregate/Luggage.cs
using AirTicketSystem.modules.luggage.Domain.ValueObjects;

namespace AirTicketSystem.modules.luggage.Domain.aggregate;

public sealed class Luggage
{
    public int Id { get; private set; }
    public int PasajeroReservaId { get; private set; }
    public int VueloId { get; private set; }
    public int TipoEquipajeId { get; private set; }
    public DescripcionLuggage? Descripcion { get; private set; }

    // Medidas declaradas al reservar
    public PesoDeclaradoKgLuggage? PesoDeclaradoKg { get; private set; }
    public LargoDeclaradoCmLuggage? LargoDeclaradoCm { get; private set; }
    public AnchoDeclaradoCmLuggage? AnchoDeclaradoCm { get; private set; }
    public AltoDeclaradoCmLuggage? AltoDeclaradoCm { get; private set; }

    // Medidas reales registradas en check-in
    public PesoRealKgLuggage? PesoRealKg { get; private set; }
    public LargoRealCmLuggage? LargoRealCm { get; private set; }
    public AnchoRealCmLuggage? AnchoRealCm { get; private set; }
    public AltoRealCmLuggage? AltoRealCm { get; private set; }

    public CodigoEquipajeLuggage? CodigoEquipaje { get; private set; }
    public CostoAdicionalLuggage CostoAdicional { get; private set; } = null!;
    public EstadoLuggage Estado { get; private set; } = null!;

    private Luggage() { }

    public static Luggage Crear(
        int pasajeroReservaId,
        int vueloId,
        int tipoEquipajeId,
        string? descripcion = null,
        decimal? pesoDeclaradoKg = null,
        int? largoDeclaradoCm = null,
        int? anchoDeclaradoCm = null,
        int? altoDeclaradoCm = null)
    {
        if (pasajeroReservaId <= 0)
            throw new ArgumentException(
                "El pasajero de reserva es obligatorio.");

        if (vueloId <= 0)
            throw new ArgumentException("El vuelo es obligatorio.");

        if (tipoEquipajeId <= 0)
            throw new ArgumentException("El tipo de equipaje es obligatorio.");

        return new Luggage
        {
            PasajeroReservaId = pasajeroReservaId,
            VueloId           = vueloId,
            TipoEquipajeId    = tipoEquipajeId,
            Descripcion       = descripcion is not null
                ? DescripcionLuggage.Crear(descripcion)
                : null,
            PesoDeclaradoKg = pesoDeclaradoKg is not null
                ? PesoDeclaradoKgLuggage.Crear(pesoDeclaradoKg.Value)
                : null,
            LargoDeclaradoCm = largoDeclaradoCm is not null
                ? LargoDeclaradoCmLuggage.Crear(largoDeclaradoCm.Value)
                : null,
            AnchoDeclaradoCm = anchoDeclaradoCm is not null
                ? AnchoDeclaradoCmLuggage.Crear(anchoDeclaradoCm.Value)
                : null,
            AltoDeclaradoCm = altoDeclaradoCm is not null
                ? AltoDeclaradoCmLuggage.Crear(altoDeclaradoCm.Value)
                : null,
            CodigoEquipaje = null,
            CostoAdicional = CostoAdicionalLuggage.SinCosto(),
            Estado         = EstadoLuggage.Declarado()
        };
    }

    // ── Check-in — registro de medidas reales ────────────────

    /// <summary>
    /// Registra las medidas reales del equipaje durante el check-in,
    /// asigna el código físico y calcula el costo adicional si aplica.
    /// </summary>
    public void RegistrarEnCheckin(
        decimal pesoRealKg,
        decimal pesoMaximoPermitido,
        decimal costoPorKgExcedido,
        int? largoRealCm = null,
        int? anchoRealCm = null,
        int? altoRealCm = null)
    {
        if (Estado.Valor != "DECLARADO")
            throw new InvalidOperationException(
                "Solo se puede registrar en check-in un equipaje declarado.");

        // Registrar medidas reales
        PesoRealKg   = PesoRealKgLuggage.Crear(pesoRealKg);
        LargoRealCm  = largoRealCm is not null
            ? LargoRealCmLuggage.Crear(largoRealCm.Value)
            : null;
        AnchoRealCm  = anchoRealCm is not null
            ? AnchoRealCmLuggage.Crear(anchoRealCm.Value)
            : null;
        AltoRealCm   = altoRealCm is not null
            ? AltoRealCmLuggage.Crear(altoRealCm.Value)
            : null;

        // Calcular costo adicional por exceso de peso
        CostoAdicional = CostoAdicionalLuggage.PorExcesoDePeso(
            pesoRealKg,
            pesoMaximoPermitido,
            costoPorKgExcedido);

        // Generar código físico del equipaje
        CodigoEquipaje = CodigoEquipajeLuggage.Generar();

        // Avanzar estado
        CambiarEstado(EstadoLuggage.Registrado());
    }

    // ── Máquina de estados ───────────────────────────────────

    public void EnviarABodega()
    {
        if (!Estado.PuedeTransicionarA(EstadoLuggage.EnBodega()))
            throw new InvalidOperationException(
                $"No se puede enviar a bodega el equipaje en estado '{Estado}'.");

        if (CodigoEquipaje is null)
            throw new InvalidOperationException(
                "El equipaje debe tener código asignado antes de enviarse a bodega.");

        CambiarEstado(EstadoLuggage.EnBodega());
    }

    public void RegistrarEnDestino()
    {
        if (!Estado.PuedeTransicionarA(EstadoLuggage.EnDestino()))
            throw new InvalidOperationException(
                $"No se puede registrar en destino el equipaje en estado '{Estado}'.");

        CambiarEstado(EstadoLuggage.EnDestino());
    }

    public void RegistrarEntrega()
    {
        if (!Estado.PuedeTransicionarA(EstadoLuggage.Entregado()))
            throw new InvalidOperationException(
                $"No se puede registrar entrega del equipaje en estado '{Estado}'.");

        CambiarEstado(EstadoLuggage.Entregado());
    }

    public void ReportarPerdido()
    {
        if (!Estado.PuedeTransicionarA(EstadoLuggage.Perdido()))
            throw new InvalidOperationException(
                $"No se puede reportar como perdido el equipaje en estado '{Estado}'.");

        CambiarEstado(EstadoLuggage.Perdido());
    }

    public void ReportarDanado()
    {
        if (!Estado.PuedeTransicionarA(EstadoLuggage.Danado()))
            throw new InvalidOperationException(
                $"No se puede reportar como dañado el equipaje en estado '{Estado}'.");

        CambiarEstado(EstadoLuggage.Danado());
    }

    private void CambiarEstado(EstadoLuggage nuevoEstado)
    {
        if (!Estado.PuedeTransicionarA(nuevoEstado))
            throw new InvalidOperationException(
                $"Transición de estado inválida: '{Estado}' → '{nuevoEstado}'.");

        Estado = nuevoEstado;
    }

    // ── Propiedades de negocio ───────────────────────────────

    public bool EstaRegistrado =>
        CodigoEquipaje is not null;

    public bool EstaEnTransito =>
        Estado.EstaEnTransito;

    public bool EstaFinalizado =>
        Estado.EstaFinalizado;

    public bool TieneIncidencia =>
        Estado.TieneIncidencia;

    public bool TieneCostoAdicional =>
        CostoAdicional.TieneCosto;

    public bool TieneMedidasDeclaradas =>
        PesoDeclaradoKg is not null;

    public bool TieneMedidasReales =>
        PesoRealKg is not null;

    /// <summary>
    /// Indica si el peso real supera al declarado.
    /// Útil para detectar subdeclaraciones al momento del check-in.
    /// </summary>
    public bool PesoRealSuperaDeclarado
    {
        get
        {
            if (PesoRealKg is null || PesoDeclaradoKg is null)
                return false;

            return PesoRealKg.ExcedePesoDeclarado(PesoDeclaradoKg.Valor);
        }
    }

    /// <summary>
    /// Diferencia entre el peso real y el declarado.
    /// Retorna null si no hay medidas de ambos tipos.
    /// </summary>
    public decimal? DiferenciaKgConDeclarado
    {
        get
        {
            if (PesoRealKg is null || PesoDeclaradoKg is null)
                return null;

            return PesoRealKg.DiferenciaConDeclarado(PesoDeclaradoKg.Valor);
        }
    }

    public string ResumenMedidasDeclaradas
    {
        get
        {
            if (!TieneMedidasDeclaradas)
                return "Sin medidas declaradas";

            var partes = new List<string>();

            if (PesoDeclaradoKg is not null)
                partes.Add($"Peso: {PesoDeclaradoKg}");

            if (LargoDeclaradoCm is not null &&
                AnchoDeclaradoCm is not null &&
                AltoDeclaradoCm is not null)
                partes.Add(
                    $"Dimensiones: {LargoDeclaradoCm} x " +
                    $"{AnchoDeclaradoCm} x {AltoDeclaradoCm}");

            return string.Join(" | ", partes);
        }
    }

    public string ResumenMedidasReales
    {
        get
        {
            if (!TieneMedidasReales)
                return "Sin medidas reales registradas";

            var partes = new List<string>();

            if (PesoRealKg is not null)
                partes.Add($"Peso real: {PesoRealKg}");

            if (LargoRealCm is not null &&
                AnchoRealCm is not null &&
                AltoRealCm is not null)
                partes.Add(
                    $"Dimensiones reales: {LargoRealCm} x " +
                    $"{AnchoRealCm} x {AltoRealCm}");

            return string.Join(" | ", partes);
        }
    }

    public static Luggage Reconstituir(
        int id,
        int pasajeroReservaId,
        int vueloId,
        int tipoEquipajeId,
        string? descripcion,
        decimal? pesoDeclaradoKg,
        int? largoDeclaradoCm,
        int? anchoDeclaradoCm,
        int? altoDeclaradoCm,
        decimal? pesoRealKg,
        int? largoRealCm,
        int? anchoRealCm,
        int? altoRealCm,
        string? codigoEquipaje,
        decimal costoAdicional,
        string estado)
    {
        var luggage = new Luggage
        {
            PasajeroReservaId = pasajeroReservaId,
            VueloId           = vueloId,
            TipoEquipajeId    = tipoEquipajeId,
            Descripcion       = descripcion is not null
                ? DescripcionLuggage.Crear(descripcion)
                : null,
            PesoDeclaradoKg = pesoDeclaradoKg is not null
                ? PesoDeclaradoKgLuggage.Crear(pesoDeclaradoKg.Value)
                : null,
            LargoDeclaradoCm = largoDeclaradoCm is not null
                ? LargoDeclaradoCmLuggage.Crear(largoDeclaradoCm.Value)
                : null,
            AnchoDeclaradoCm = anchoDeclaradoCm is not null
                ? AnchoDeclaradoCmLuggage.Crear(anchoDeclaradoCm.Value)
                : null,
            AltoDeclaradoCm = altoDeclaradoCm is not null
                ? AltoDeclaradoCmLuggage.Crear(altoDeclaradoCm.Value)
                : null,
            PesoRealKg = pesoRealKg is not null
                ? PesoRealKgLuggage.Crear(pesoRealKg.Value)
                : null,
            LargoRealCm = largoRealCm is not null
                ? LargoRealCmLuggage.Crear(largoRealCm.Value)
                : null,
            AnchoRealCm = anchoRealCm is not null
                ? AnchoRealCmLuggage.Crear(anchoRealCm.Value)
                : null,
            AltoRealCm = altoRealCm is not null
                ? AltoRealCmLuggage.Crear(altoRealCm.Value)
                : null,
            CodigoEquipaje = codigoEquipaje is not null
                ? CodigoEquipajeLuggage.Crear(codigoEquipaje)
                : null,
            CostoAdicional = CostoAdicionalLuggage.Crear(costoAdicional),
            Estado         = EstadoLuggage.Crear(estado)
        };
        luggage.Id = id;
        return luggage;
    }

    public void EstablecerId(int id) => Id = id;

    public override string ToString() =>
        $"Equipaje [{CodigoEquipaje?.Valor ?? "Sin código"}] — " +
        $"{Estado} | {ResumenMedidasDeclaradas}";
}