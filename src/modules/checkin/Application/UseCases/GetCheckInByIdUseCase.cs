// src/modules/checkin/Application/UseCases/GetCheckInByIdUseCase.cs
using AirTicketSystem.modules.checkin.Domain.aggregate;
using AirTicketSystem.modules.checkin.Domain.Repositories;

namespace AirTicketSystem.modules.checkin.Application.UseCases;

public sealed class GetCheckInByIdUseCase
{
    private readonly ICheckInRepository _repository;

    public GetCheckInByIdUseCase(ICheckInRepository repository) => _repository = repository;

    public async Task<CheckIn> ExecuteAsync(
        int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del check-in no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró un check-in con ID {id}.");
    }
}
