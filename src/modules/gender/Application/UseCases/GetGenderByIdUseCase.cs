// src/modules/gender/Application/UseCases/GetGenderByIdUseCase.cs
using AirTicketSystem.modules.gender.Domain.aggregate;
using AirTicketSystem.modules.gender.Domain.Repositories;

namespace AirTicketSystem.modules.gender.Application.UseCases;

public sealed class GetGenderByIdUseCase
{
    private readonly IGenderRepository _repository;

    public GetGenderByIdUseCase(IGenderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Gender> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del género no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un género con ID {id}.");
    }
}
