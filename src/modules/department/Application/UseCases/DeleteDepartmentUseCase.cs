// src/modules/department/Application/UseCases/DeleteDepartmentUseCase.cs
using AirTicketSystem.modules.department.Domain.Repositories;

namespace AirTicketSystem.modules.department.Application.UseCases;

public sealed class DeleteDepartmentUseCase
{
    private readonly IDepartmentRepository _repository;

    public DeleteDepartmentUseCase(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        _ = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un departamento con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}
