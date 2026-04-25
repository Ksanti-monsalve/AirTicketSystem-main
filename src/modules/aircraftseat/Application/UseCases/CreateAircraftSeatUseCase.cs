// src/modules/aircraftseat/Application/UseCases/CreateAircraftSeatUseCase.cs
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;
using AirTicketSystem.modules.aircraftseat.Domain.aggregate;
using AirTicketSystem.modules.aircraftseat.Domain.ValueObjects;
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.serviceclass.Domain.Repositories;

namespace AirTicketSystem.modules.aircraftseat.Application.UseCases;

public class CreateAircraftSeatUseCase
{
    private readonly IAircraftSeatRepository _repository;
    private readonly IAircraftRepository _aircraftRepository;
    private readonly IServiceClassRepository _serviceClassRepository;

    public CreateAircraftSeatUseCase(
        IAircraftSeatRepository repository,
        IAircraftRepository aircraftRepository,
        IServiceClassRepository serviceClassRepository)
    {
        _repository             = repository;
        _aircraftRepository     = aircraftRepository;
        _serviceClassRepository = serviceClassRepository;
    }

    public async Task<AircraftSeat> ExecuteAsync(
        int avionId,
        int claseServicioId,
        int fila,
        char columna,
        bool esVentana,
        bool esPasillo,
        decimal costoSeleccion,
        CancellationToken cancellationToken = default)
    {
        var avion = await _aircraftRepository.FindByIdAsync(avionId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un avión con ID {avionId}.");

        if (!avion.Activo.Valor)
            throw new InvalidOperationException(
                $"El avión '{avion.Matricula.Valor}' está dado de baja. " +
                "No se pueden agregar asientos.");

        _ = await _serviceClassRepository.FindByIdAsync(claseServicioId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una clase de servicio con ID {claseServicioId}.");

        if (esVentana && esPasillo)
            throw new InvalidOperationException(
                "Un asiento no puede ser de ventana y de pasillo al mismo tiempo.");

        var filaVO    = FilaAircraftSeat.Crear(fila);
        var columnaVO = ColumnaAircraftSeat.Crear(columna);
        var codigoVO  = CodigoAsientoAircraftSeat.Crear(fila, columna);
        var costoVO   = CostoSeleccionAircraftSeat.Crear(costoSeleccion);

        if (await _repository.ExistsByCodigoAndAvionAsync(codigoVO.Valor, avionId))
            throw new InvalidOperationException(
                $"Ya existe el asiento '{codigoVO.Valor}' en el avión " +
                $"'{avion.Matricula.Valor}'.");

        var seat = AircraftSeat.Crear(
            avionId,
            claseServicioId,
            filaVO.Valor,
            columnaVO.Valor,
            esVentana,
            esPasillo);

        seat.ActualizarCondiciones(esVentana, esPasillo, costoVO.Valor);

        await _repository.SaveAsync(seat);
        return seat;
    }
}