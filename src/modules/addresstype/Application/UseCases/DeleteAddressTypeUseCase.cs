// src/modules/addresstype/Application/UseCases/DeleteAddressTypeUseCase.cs
using AirTicketSystem.modules.addresstype.Domain.Repositories;

namespace AirTicketSystem.modules.addresstype.Application.UseCases;

public sealed class DeleteAddressTypeUseCase
{
    private readonly IAddressTypeRepository _repository;

    public DeleteAddressTypeUseCase(IAddressTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        _ = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de dirección con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}
