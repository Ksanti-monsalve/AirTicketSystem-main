// src/modules/luggagerestriction/Application/UseCases/DeleteLuggageRestrictionUseCase.cs
using AirTicketSystem.modules.luggagerestriction.Domain.Repositories;

namespace AirTicketSystem.modules.luggagerestriction.Application.UseCases;

public sealed class DeleteLuggageRestrictionUseCase
{
    private readonly ILuggageRestrictionRepository _repository;

    public DeleteLuggageRestrictionUseCase(ILuggageRestrictionRepository repository)
        => _repository = repository;

    public async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        _ = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una restricción de equipaje con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}
