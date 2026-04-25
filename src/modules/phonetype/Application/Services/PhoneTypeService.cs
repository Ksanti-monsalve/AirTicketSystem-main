// src/modules/phonetype/Application/Services/PhoneTypeService.cs
using AirTicketSystem.modules.phonetype.Application.Interfaces;
using AirTicketSystem.modules.phonetype.Application.UseCases;
using AirTicketSystem.modules.phonetype.Domain.aggregate;

namespace AirTicketSystem.modules.phonetype.Application.Services;

public sealed class PhoneTypeService : IPhoneTypeService
{
    private readonly CreatePhoneTypeUseCase _create;
    private readonly GetPhoneTypeByIdUseCase _getById;
    private readonly GetAllPhoneTypesUseCase _getAll;
    private readonly UpdatePhoneTypeUseCase _update;
    private readonly DeletePhoneTypeUseCase _delete;

    public PhoneTypeService(
        CreatePhoneTypeUseCase create,
        GetPhoneTypeByIdUseCase getById,
        GetAllPhoneTypesUseCase getAll,
        UpdatePhoneTypeUseCase update,
        DeletePhoneTypeUseCase delete)
    {
        _create  = create;
        _getById = getById;
        _getAll  = getAll;
        _update  = update;
        _delete  = delete;
    }

    public Task<PhoneType> CreateAsync(string descripcion)
        => _create.ExecuteAsync(descripcion);

    public Task<PhoneType> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<PhoneType>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<PhoneType> UpdateAsync(int id, string descripcion)
        => _update.ExecuteAsync(id, descripcion);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}
