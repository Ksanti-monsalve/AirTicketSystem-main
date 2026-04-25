// src/modules/checkin/Application/Services/CheckInService.cs
using AirTicketSystem.modules.checkin.Application.Interfaces;
using AirTicketSystem.modules.checkin.Application.UseCases;
using AirTicketSystem.modules.checkin.Domain.aggregate;

namespace AirTicketSystem.modules.checkin.Application.Services;

public sealed class CheckInService : ICheckInService
{
    private readonly CreateVirtualCheckInUseCase    _createVirtual;
    private readonly CreatePresentialCheckInUseCase _createPresential;
    private readonly GetCheckInByIdUseCase          _getById;
    private readonly CompleteCheckInUseCase         _complete;
    private readonly CancelCheckInUseCase           _cancel;

    public CheckInService(
        CreateVirtualCheckInUseCase    createVirtual,
        CreatePresentialCheckInUseCase createPresential,
        GetCheckInByIdUseCase          getById,
        CompleteCheckInUseCase         complete,
        CancelCheckInUseCase           cancel)
    {
        _createVirtual    = createVirtual;
        _createPresential = createPresential;
        _getById          = getById;
        _complete         = complete;
        _cancel           = cancel;
    }

    public Task<CheckIn> CreateVirtualAsync(int pasajeroReservaId)
        => _createVirtual.ExecuteAsync(pasajeroReservaId);

    public Task<CheckIn> CreatePresentialAsync(int pasajeroReservaId, int trabajadorId)
        => _createPresential.ExecuteAsync(pasajeroReservaId, trabajadorId);

    public Task<CheckIn> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<CheckIn> CompleteAsync(int id)
        => _complete.ExecuteAsync(id);

    public Task<CheckIn> CancelAsync(int id)
        => _cancel.ExecuteAsync(id);
}
