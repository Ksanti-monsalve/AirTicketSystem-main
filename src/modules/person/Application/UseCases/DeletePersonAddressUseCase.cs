// src/modules/person/Application/UseCases/DeletePersonAddressUseCase.cs
using AirTicketSystem.modules.person.Domain.Repositories;

namespace AirTicketSystem.modules.person.Application.UseCases;

public sealed class DeletePersonAddressUseCase
{
    private readonly IPersonAddressRepository _repository;

    public DeletePersonAddressUseCase(IPersonAddressRepository repository)
        => _repository = repository;

    public async Task ExecuteAsync(int addressId, CancellationToken cancellationToken = default)
    {
        if (addressId <= 0)
            throw new ArgumentException("El ID de la dirección no es válido.");

        _ = await _repository.FindByIdAsync(addressId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una dirección con ID {addressId}.");

        await _repository.DeleteAsync(addressId);
    }
}
