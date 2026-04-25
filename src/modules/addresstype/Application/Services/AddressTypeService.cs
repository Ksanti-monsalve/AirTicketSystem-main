// src/modules/addresstype/Application/Services/AddressTypeService.cs
using AirTicketSystem.modules.addresstype.Application.Interfaces;
using AirTicketSystem.modules.addresstype.Application.UseCases;
using AirTicketSystem.modules.addresstype.Domain.aggregate;

namespace AirTicketSystem.modules.addresstype.Application.Services;

public sealed class AddressTypeService : IAddressTypeService
{
    private readonly CreateAddressTypeUseCase _create;
    private readonly GetAddressTypeByIdUseCase _getById;
    private readonly GetAllAddressTypesUseCase _getAll;
    private readonly UpdateAddressTypeUseCase _update;
    private readonly DeleteAddressTypeUseCase _delete;

    public AddressTypeService(
        CreateAddressTypeUseCase create,
        GetAddressTypeByIdUseCase getById,
        GetAllAddressTypesUseCase getAll,
        UpdateAddressTypeUseCase update,
        DeleteAddressTypeUseCase delete)
    {
        _create  = create;
        _getById = getById;
        _getAll  = getAll;
        _update  = update;
        _delete  = delete;
    }

    public Task<AddressType> CreateAsync(string descripcion)
        => _create.ExecuteAsync(descripcion);

    public Task<AddressType> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<AddressType>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<AddressType> UpdateAsync(int id, string descripcion)
        => _update.ExecuteAsync(id, descripcion);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}
