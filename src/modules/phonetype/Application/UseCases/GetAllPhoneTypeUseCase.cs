// src/modules/phonetype/Application/UseCases/GetAllPhoneTypesUseCase.cs
using AirTicketSystem.modules.phonetype.Domain.aggregate;
using AirTicketSystem.modules.phonetype.Domain.Repositories;

namespace AirTicketSystem.modules.phonetype.Application.UseCases;

public sealed class GetAllPhoneTypesUseCase
{
    private readonly IPhoneTypeRepository _repository;

    public GetAllPhoneTypesUseCase(IPhoneTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<PhoneType>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindAllAsync();
    }
}
