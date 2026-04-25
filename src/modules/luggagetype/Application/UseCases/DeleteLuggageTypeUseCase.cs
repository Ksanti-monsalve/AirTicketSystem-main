// src/modules/luggagetype/Application/UseCases/DeleteLuggageTypeUseCase.cs
using AirTicketSystem.modules.luggagetype.Domain.Repositories;

namespace AirTicketSystem.modules.luggagetype.Application.UseCases;

public sealed class DeleteLuggageTypeUseCase
{
    private readonly ILuggageTypeRepository _repository;

    public DeleteLuggageTypeUseCase(ILuggageTypeRepository repository)
        => _repository = repository;

    public async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        _ = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de equipaje con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}
