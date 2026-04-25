// src/modules/luggagetype/Application/UseCases/GetLuggageTypeByIdUseCase.cs
using AirTicketSystem.modules.luggagetype.Domain.aggregate;
using AirTicketSystem.modules.luggagetype.Domain.Repositories;

namespace AirTicketSystem.modules.luggagetype.Application.UseCases;

public sealed class GetLuggageTypeByIdUseCase
{
    private readonly ILuggageTypeRepository _repository;

    public GetLuggageTypeByIdUseCase(ILuggageTypeRepository repository)
        => _repository = repository;

    public async Task<LuggageType> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tipo de equipaje no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de equipaje con ID {id}.");
    }
}
