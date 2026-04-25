// src/modules/phonetype/Application/UseCases/DeletePhoneTypeUseCase.cs
using AirTicketSystem.modules.phonetype.Domain.Repositories;

namespace AirTicketSystem.modules.phonetype.Application.UseCases;

public sealed class DeletePhoneTypeUseCase
{
    private readonly IPhoneTypeRepository _repository;

    public DeletePhoneTypeUseCase(IPhoneTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        _ = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de teléfono con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}
