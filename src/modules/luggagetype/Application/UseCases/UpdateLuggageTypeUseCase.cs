// src/modules/luggagetype/Application/UseCases/UpdateLuggageTypeUseCase.cs
using AirTicketSystem.modules.luggagetype.Domain.aggregate;
using AirTicketSystem.modules.luggagetype.Domain.Repositories;

namespace AirTicketSystem.modules.luggagetype.Application.UseCases;

public sealed class UpdateLuggageTypeUseCase
{
    private readonly ILuggageTypeRepository _repository;

    public UpdateLuggageTypeUseCase(ILuggageTypeRepository repository)
        => _repository = repository;

    public async Task<LuggageType> ExecuteAsync(int id, string nombre, CancellationToken cancellationToken = default)
    {
        var luggageType = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de equipaje con ID {id}.");

        if (await _repository.ExistsByNombreAsync(nombre) &&
            !luggageType.Nombre.Valor.Equals(nombre, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(
                $"Ya existe un tipo de equipaje con el nombre '{nombre}'.");

        luggageType.ActualizarNombre(nombre);
        await _repository.UpdateAsync(luggageType);
        return luggageType;
    }
}
