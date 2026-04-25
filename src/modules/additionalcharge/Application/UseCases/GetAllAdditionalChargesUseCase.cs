// src/modules/additionalcharge/Application/UseCases/GetAllAdditionalChargesUseCase.cs
using AirTicketSystem.modules.additionalcharge.Domain.aggregate;
using AirTicketSystem.modules.additionalcharge.Domain.Repositories;

namespace AirTicketSystem.modules.additionalcharge.Application.UseCases;

public sealed class GetAllAdditionalChargesUseCase
{
    private readonly IAdditionalChargeRepository _repository;

    public GetAllAdditionalChargesUseCase(IAdditionalChargeRepository repository)
        => _repository = repository;

    public Task<IReadOnlyCollection<AdditionalCharge>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => _repository.FindAllAsync();
}
