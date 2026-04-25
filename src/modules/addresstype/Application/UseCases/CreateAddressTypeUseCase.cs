// src/modules/addresstype/Application/UseCases/CreateAddressTypeUseCase.cs
using AirTicketSystem.modules.addresstype.Domain.aggregate;
using AirTicketSystem.modules.addresstype.Domain.Repositories;
using AirTicketSystem.modules.addresstype.Domain.ValueObjects;

namespace AirTicketSystem.modules.addresstype.Application.UseCases;

public sealed class CreateAddressTypeUseCase
{
    private readonly IAddressTypeRepository _repository;

    public CreateAddressTypeUseCase(IAddressTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddressType> ExecuteAsync(
        string descripcion,
        CancellationToken cancellationToken = default)
    {
        var descripcionVO = DescripcionAddressType.Crear(descripcion);

        if (await _repository.ExistsByDescripcionAsync(descripcionVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un tipo de dirección con la descripción '{descripcionVO.Valor}'.");

        var addressType = AddressType.Crear(descripcionVO.Valor);
        await _repository.SaveAsync(addressType);
        return addressType;
    }
}
