// src/modules/luggagerestriction/Domain/aggregate/LuggageRestriction.cs
using AirTicketSystem.modules.luggagerestriction.Domain.ValueObjects;

namespace AirTicketSystem.modules.luggagerestriction.Domain.aggregate;

public sealed class LuggageRestriction
{
    public int Id { get; private set; }
    public int TarifaId { get; private set; }
    public int TipoEquipajeId { get; private set; }
    public PiezasIncluidasLuggageRestriction PiezasIncluidas { get; private set; } = null!;
    public PesoMaximoKgLuggageRestriction PesoMaximoKg { get; private set; } = null!;
    public LargoMaxCmLuggageRestriction? LargoMaxCm { get; private set; }
    public AnchoMaxCmLuggageRestriction? AnchoMaxCm { get; private set; }
    public AltoMaxCmLuggageRestriction? AltoMaxCm { get; private set; }
    public CostoExcesoKgLuggageRestriction CostoExcesoKg { get; private set; } = null!;

    private LuggageRestriction() { }

    public static LuggageRestriction Reconstituir(
        int id,
        int tarifaId,
        int tipoEquipajeId,
        int piezasIncluidas,
        decimal pesoMaximoKg,
        decimal costoExcesoKg,
        int? largoMaxCm,
        int? anchoMaxCm,
        int? altoMaxCm)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la restricción no es válido.");

        return new LuggageRestriction
        {
            Id              = id,
            TarifaId        = tarifaId,
            TipoEquipajeId  = tipoEquipajeId,
            PiezasIncluidas = PiezasIncluidasLuggageRestriction.Crear(piezasIncluidas),
            PesoMaximoKg    = PesoMaximoKgLuggageRestriction.Crear(pesoMaximoKg),
            CostoExcesoKg   = CostoExcesoKgLuggageRestriction.Crear(costoExcesoKg),
            LargoMaxCm      = largoMaxCm is not null
                ? LargoMaxCmLuggageRestriction.Crear(largoMaxCm.Value)
                : null,
            AnchoMaxCm = anchoMaxCm is not null
                ? AnchoMaxCmLuggageRestriction.Crear(anchoMaxCm.Value)
                : null,
            AltoMaxCm = altoMaxCm is not null
                ? AltoMaxCmLuggageRestriction.Crear(altoMaxCm.Value)
                : null
        };
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la restricción no es válido.");

        Id = id;
    }

    public static LuggageRestriction Crear(
        int tarifaId,
        int tipoEquipajeId,
        int piezasIncluidas,
        decimal pesoMaximoKg,
        decimal costoExcesoKg,
        int? largoMaxCm = null,
        int? anchoMaxCm = null,
        int? altoMaxCm = null)
    {
        if (tarifaId <= 0)
            throw new ArgumentException("La tarifa es obligatoria.");

        if (tipoEquipajeId <= 0)
            throw new ArgumentException("El tipo de equipaje es obligatorio.");

        return new LuggageRestriction
        {
            TarifaId        = tarifaId,
            TipoEquipajeId  = tipoEquipajeId,
            PiezasIncluidas = PiezasIncluidasLuggageRestriction.Crear(piezasIncluidas),
            PesoMaximoKg    = PesoMaximoKgLuggageRestriction.Crear(pesoMaximoKg),
            CostoExcesoKg   = CostoExcesoKgLuggageRestriction.Crear(costoExcesoKg),
            LargoMaxCm      = largoMaxCm is not null
                ? LargoMaxCmLuggageRestriction.Crear(largoMaxCm.Value)
                : null,
            AnchoMaxCm = anchoMaxCm is not null
                ? AnchoMaxCmLuggageRestriction.Crear(anchoMaxCm.Value)
                : null,
            AltoMaxCm = altoMaxCm is not null
                ? AltoMaxCmLuggageRestriction.Crear(altoMaxCm.Value)
                : null
        };
    }

    public void ActualizarPeso(decimal pesoMaximoKg, decimal costoExcesoKg)
    {
        PesoMaximoKg  = PesoMaximoKgLuggageRestriction.Crear(pesoMaximoKg);
        CostoExcesoKg = CostoExcesoKgLuggageRestriction.Crear(costoExcesoKg);
    }

    public void ActualizarDimensiones(
        int? largoMaxCm,
        int? anchoMaxCm,
        int? altoMaxCm)
    {
        LargoMaxCm = largoMaxCm is not null
            ? LargoMaxCmLuggageRestriction.Crear(largoMaxCm.Value)
            : null;
        AnchoMaxCm = anchoMaxCm is not null
            ? AnchoMaxCmLuggageRestriction.Crear(anchoMaxCm.Value)
            : null;
        AltoMaxCm = altoMaxCm is not null
            ? AltoMaxCmLuggageRestriction.Crear(altoMaxCm.Value)
            : null;
    }

    public void ActualizarPiezasIncluidas(int piezasIncluidas)
    {
        PiezasIncluidas = PiezasIncluidasLuggageRestriction.Crear(piezasIncluidas);
    }

    // ── Propiedades de negocio ───────────────────────────────

    public bool TieneDimensionesDefinidas =>
        LargoMaxCm is not null &&
        AnchoMaxCm is not null &&
        AltoMaxCm is not null;

    public bool IncluyePiezas => PiezasIncluidas.IncluyePiezas;

    public bool TieneCostoExceso => CostoExcesoKg.TieneCosto;

    /// <summary>
    /// Calcula el costo adicional por exceso de peso de un equipaje.
    /// </summary>
    public decimal CalcularCostoPorPeso(decimal pesoRealKg)
    {
        var excedente = PesoMaximoKg.ExcedenteDe(pesoRealKg);
        return CostoExcesoKg.CalcularCostoExcedente(excedente);
    }

    /// <summary>
    /// Verifica si un equipaje cumple con todas las restricciones
    /// de peso y dimensiones de esta tarifa.
    /// </summary>
    public bool EquipajeEsValido(
        decimal pesoKg,
        int? largoKg = null,
        int? anchoCm = null,
        int? altoCm = null)
    {
        if (PesoMaximoKg.ExcedePeso(pesoKg))
            return false;

        if (TieneDimensionesDefinidas)
        {
            if (largoKg is not null && LargoMaxCm!.ExcedeLargo(largoKg.Value))
                return false;

            if (anchoCm is not null && AnchoMaxCm!.ExcedeAncho(anchoCm.Value))
                return false;

            if (altoCm is not null && AltoMaxCm!.ExcedeAlto(altoCm.Value))
                return false;
        }

        return true;
    }

    public override string ToString() =>
        $"Restricción Tarifa #{TarifaId} — " +
        $"Máx {PesoMaximoKg} | {PiezasIncluidas}";
}