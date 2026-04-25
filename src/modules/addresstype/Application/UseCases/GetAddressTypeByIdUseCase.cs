// src/modules/addresstype/Application/UseCases/GetAddressTypeByIdUseCase.cs
using AirTicketSystem.modules.addresstype.Domain.aggregate;
using AirTicketSystem.modules.addresstype.Domain.Repositories;

namespace AirTicketSystem.modules.addresstype.Application.UseCases;

public sealed class GetAddressTypeByIdUseCase
{
    private readonly IAddressTypeRepository _repository;

    public GetAddressTypeByIdUseCase(IAddressTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddressType> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de dirección con ID {id}.");
    }
}
