// src/modules/luggagetype/Application/UseCases/CreateLuggageTypeUseCase.cs
using AirTicketSystem.modules.luggagetype.Domain.aggregate;
using AirTicketSystem.modules.luggagetype.Domain.Repositories;

namespace AirTicketSystem.modules.luggagetype.Application.UseCases;

public sealed class CreateLuggageTypeUseCase
{
    private readonly ILuggageTypeRepository _repository;

    public CreateLuggageTypeUseCase(ILuggageTypeRepository repository)
        => _repository = repository;

    public async Task<LuggageType> ExecuteAsync(string nombre, CancellationToken cancellationToken = default)
    {
        if (await _repository.ExistsByNombreAsync(nombre))
            throw new InvalidOperationException(
                $"Ya existe un tipo de equipaje con el nombre '{nombre}'.");

        var luggageType = LuggageType.Crear(nombre);
        await _repository.SaveAsync(luggageType);
        return luggageType;
    }
}
