// src/modules/additionalcharge/Application/UseCases/DeleteAdditionalChargeUseCase.cs
using AirTicketSystem.modules.additionalcharge.Domain.Repositories;

namespace AirTicketSystem.modules.additionalcharge.Application.UseCases;

public sealed class DeleteAdditionalChargeUseCase
{
    private readonly IAdditionalChargeRepository _repository;

    public DeleteAdditionalChargeUseCase(IAdditionalChargeRepository repository)
        => _repository = repository;

    public async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        _ = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un cargo adicional con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}
