// src/modules/aircraftseat/Domain/aggregate/AircraftSeat.cs
using AirTicketSystem.modules.aircraftseat.Domain.ValueObjects;

namespace AirTicketSystem.modules.aircraftseat.Domain.aggregate;

public sealed class AircraftSeat
{
    public int Id { get; private set; }
    public int AvionId { get; private set; }
    public int ClaseServicioId { get; private set; }
    public CodigoAsientoAircraftSeat CodigoAsiento { get; private set; } = null!;
    public FilaAircraftSeat Fila { get; private set; } = null!;
    public ColumnaAircraftSeat Columna { get; private set; } = null!;
    public EsVentanaAircraftSeat EsVentana { get; private set; } = null!;
    public EsPasilloAircraftSeat EsPasillo { get; private set; } = null!;
    public ActivoAircraftSeat Activo { get; private set; } = null!;
    public CostoSeleccionAircraftSeat CostoSeleccion { get; private set; } = null!;
    public bool TieneCostoDeSeleccion => CostoSeleccion.Valor > 0;

    private AircraftSeat() { }

    public static AircraftSeat Crear(
        int avionId,
        int claseServicioId,
        int fila,
        char columna,
        bool esVentana = false,
        bool esPasillo = false)
    {
        if (avionId <= 0)
            throw new ArgumentException("El avión es obligatorio.");

        if (claseServicioId <= 0)
            throw new ArgumentException("La clase de servicio es obligatoria.");

        // Un asiento no puede ser ventana y pasillo al mismo tiempo
        if (esVentana && esPasillo)
            throw new InvalidOperationException(
                "Un asiento no puede ser de ventana y de pasillo al mismo tiempo.");

        var filaVO    = FilaAircraftSeat.Crear(fila);
        var columnaVO = ColumnaAircraftSeat.Crear(columna);

        return new AircraftSeat
        {
            AvionId          = avionId,
            ClaseServicioId  = claseServicioId,
            CodigoAsiento    = CodigoAsientoAircraftSeat.Crear(fila, columna),
            Fila             = filaVO,
            Columna          = columnaVO,
            EsVentana        = EsVentanaAircraftSeat.Crear(esVentana),
            EsPasillo        = EsPasilloAircraftSeat.Crear(esPasillo),
            Activo           = ActivoAircraftSeat.Activo(),
            CostoSeleccion   = CostoSeleccionAircraftSeat.Crear(0)
        };
    }

    public static AircraftSeat Reconstituir(
        int id,
        int avionId,
        int claseServicioId,
        string codigoAsiento,
        int fila,
        string columna,
        bool esVentana,
        bool esPasillo,
        bool activo,
        decimal costoSeleccion)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del asiento no es válido.");

        var filaVO    = FilaAircraftSeat.Crear(fila);
        var columnaVO = ColumnaAircraftSeat.Crear(columna);

        var seat = new AircraftSeat
        {
            Id              = id,
            AvionId         = avionId,
            ClaseServicioId = claseServicioId,
            CodigoAsiento   = CodigoAsientoAircraftSeat.Crear(codigoAsiento),
            Fila            = filaVO,
            Columna         = columnaVO,
            EsVentana       = EsVentanaAircraftSeat.Crear(esVentana),
            EsPasillo       = EsPasilloAircraftSeat.Crear(esPasillo),
            Activo          = ActivoAircraftSeat.Crear(activo),
            CostoSeleccion  = CostoSeleccionAircraftSeat.Crear(costoSeleccion)
        };
        return seat;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del asiento no es valido.");

        Id = id;
    }

    public void ActualizarCondiciones(
        bool esVentana,
        bool esPasillo,
        decimal costoSeleccion)
    {
        if (esVentana && esPasillo)
            throw new InvalidOperationException(
                "Un asiento no puede ser de ventana y de pasillo al mismo tiempo.");

        EsVentana = EsVentanaAircraftSeat.Crear(esVentana);
        EsPasillo = EsPasilloAircraftSeat.Crear(esPasillo);
        CostoSeleccion = CostoSeleccionAircraftSeat.Crear(costoSeleccion);
    }

    public void Activar()
    {
        if (Activo.Valor)
            throw new InvalidOperationException(
                "El asiento ya se encuentra activo.");

        Activo = ActivoAircraftSeat.Activo();
    }

    public void Desactivar()
    {
        if (!Activo.Valor)
            throw new InvalidOperationException(
                "El asiento ya se encuentra inactivo.");

        Activo = ActivoAircraftSeat.Inactivo();
    }

    // Propiedades de negocio
    public bool EstaDisponibleParaAsignar => Activo.Valor;

    public string UbicacionDescriptiva
    {
        get
        {
            if (EsVentana.Valor) return "Ventana";
            if (EsPasillo.Valor) return "Pasillo";
            return "Centro";
        }
    }

    public override string ToString() =>
        $"Asiento {CodigoAsiento} — {UbicacionDescriptiva}";
}