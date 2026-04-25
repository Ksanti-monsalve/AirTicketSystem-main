// src/modules/pilotrating/Application/UseCases/DeletePilotRatingUseCase.cs
using AirTicketSystem.modules.pilotrating.Domain.Repositories;

namespace AirTicketSystem.modules.pilotrating.Application.UseCases;

public sealed class DeletePilotRatingUseCase
{
    private readonly IPilotRatingRepository _repository;

    public DeletePilotRatingUseCase(IPilotRatingRepository repository)
        => _repository = repository;

    public async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        _ = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una habilitación con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}
