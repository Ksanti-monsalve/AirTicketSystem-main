// src/modules/addresstype/Application/UseCases/UpdateAddressTypeUseCase.cs
using AirTicketSystem.modules.addresstype.Domain.aggregate;
using AirTicketSystem.modules.addresstype.Domain.Repositories;
using AirTicketSystem.modules.addresstype.Domain.ValueObjects;

namespace AirTicketSystem.modules.addresstype.Application.UseCases;

public sealed class UpdateAddressTypeUseCase
{
    private readonly IAddressTypeRepository _repository;

    public UpdateAddressTypeUseCase(IAddressTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddressType> ExecuteAsync(
        int id,
        string descripcion,
        CancellationToken cancellationToken = default)
    {
        var addressType = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de dirección con ID {id}.");

        var descripcionVO = DescripcionAddressType.Crear(descripcion);

        if (!string.Equals(descripcionVO.Valor, addressType.Descripcion.Valor, StringComparison.OrdinalIgnoreCase) &&
            await _repository.ExistsByDescripcionAsync(descripcionVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro tipo de dirección con la descripción '{descripcionVO.Valor}'.");

        addressType.ActualizarDescripcion(descripcionVO.Valor);
        await _repository.UpdateAsync(addressType);
        return addressType;
    }
}
