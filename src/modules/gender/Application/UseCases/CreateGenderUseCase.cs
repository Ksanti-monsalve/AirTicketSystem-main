// src/modules/gender/Application/UseCases/CreateGenderUseCase.cs
using AirTicketSystem.modules.gender.Domain.aggregate;
using AirTicketSystem.modules.gender.Domain.Repositories;
using AirTicketSystem.modules.gender.Domain.ValueObjects;

namespace AirTicketSystem.modules.gender.Application.UseCases;

public sealed class CreateGenderUseCase
{
    private readonly IGenderRepository _repository;

    public CreateGenderUseCase(IGenderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Gender> ExecuteAsync(
        string nombre,
        CancellationToken cancellationToken = default)
    {
        var nombreVO = NombreGender.Crear(nombre);

        if (await _repository.ExistsByNombreAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un género con el nombre '{nombreVO.Valor}'.");

        var gender = Gender.Crear(nombreVO.Valor);
        await _repository.SaveAsync(gender);
        return gender;
    }
}
