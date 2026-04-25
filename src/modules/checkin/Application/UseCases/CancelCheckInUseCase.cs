// src/modules/checkin/Application/UseCases/CancelCheckInUseCase.cs
using AirTicketSystem.modules.checkin.Domain.aggregate;
using AirTicketSystem.modules.checkin.Domain.Repositories;

namespace AirTicketSystem.modules.checkin.Application.UseCases;

public sealed class CancelCheckInUseCase
{
    private readonly ICheckInRepository _repository;

    public CancelCheckInUseCase(ICheckInRepository repository) => _repository = repository;

    public async Task<CheckIn> ExecuteAsync(
        int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del check-in no es válido.");

        var checkIn = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró un check-in con ID {id}.");

        checkIn.Cancelar();
        await _repository.UpdateAsync(checkIn);
        return checkIn;
    }
}
