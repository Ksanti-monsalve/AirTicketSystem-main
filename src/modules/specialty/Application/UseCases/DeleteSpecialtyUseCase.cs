// src/modules/specialty/Application/UseCases/DeleteSpecialtyUseCase.cs
using AirTicketSystem.modules.specialty.Domain.Repositories;

namespace AirTicketSystem.modules.specialty.Application.UseCases;

public class DeleteSpecialtyUseCase
{
    private readonly ISpecialtyRepository _repository;

    public DeleteSpecialtyUseCase(ISpecialtyRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        _ = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una especialidad con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}