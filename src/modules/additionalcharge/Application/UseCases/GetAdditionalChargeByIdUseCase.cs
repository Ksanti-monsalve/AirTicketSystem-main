// src/modules/additionalcharge/Application/UseCases/GetAdditionalChargeByIdUseCase.cs
using AirTicketSystem.modules.additionalcharge.Domain.aggregate;
using AirTicketSystem.modules.additionalcharge.Domain.Repositories;

namespace AirTicketSystem.modules.additionalcharge.Application.UseCases;

public sealed class GetAdditionalChargeByIdUseCase
{
    private readonly IAdditionalChargeRepository _repository;

    public GetAdditionalChargeByIdUseCase(IAdditionalChargeRepository repository)
        => _repository = repository;

    public async Task<AdditionalCharge> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del cargo adicional no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un cargo adicional con ID {id}.");
    }
}
