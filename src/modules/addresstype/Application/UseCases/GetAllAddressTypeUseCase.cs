// src/modules/addresstype/Application/UseCases/GetAllAddressTypesUseCase.cs
using AirTicketSystem.modules.addresstype.Domain.aggregate;
using AirTicketSystem.modules.addresstype.Domain.Repositories;

namespace AirTicketSystem.modules.addresstype.Application.UseCases;

public sealed class GetAllAddressTypesUseCase
{
    private readonly IAddressTypeRepository _repository;

    public GetAllAddressTypesUseCase(IAddressTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<AddressType>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindAllAsync();
    }
}
