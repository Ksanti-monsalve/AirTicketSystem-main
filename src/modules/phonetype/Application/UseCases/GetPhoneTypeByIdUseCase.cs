// src/modules/phonetype/Application/UseCases/GetPhoneTypeByIdUseCase.cs
using AirTicketSystem.modules.phonetype.Domain.aggregate;
using AirTicketSystem.modules.phonetype.Domain.Repositories;

namespace AirTicketSystem.modules.phonetype.Application.UseCases;

public sealed class GetPhoneTypeByIdUseCase
{
    private readonly IPhoneTypeRepository _repository;

    public GetPhoneTypeByIdUseCase(IPhoneTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<PhoneType> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de teléfono con ID {id}.");
    }
}
