// src/modules/gender/Application/UseCases/UpdateGenderUseCase.cs
using AirTicketSystem.modules.gender.Domain.aggregate;
using AirTicketSystem.modules.gender.Domain.Repositories;
using AirTicketSystem.modules.gender.Domain.ValueObjects;

namespace AirTicketSystem.modules.gender.Application.UseCases;

public sealed class UpdateGenderUseCase
{
    private readonly IGenderRepository _repository;

    public UpdateGenderUseCase(IGenderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Gender> ExecuteAsync(
        int id,
        string nombre,
        CancellationToken cancellationToken = default)
    {
        var gender = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un género con ID {id}.");

        var nombreVO = NombreGender.Crear(nombre);

        if (!string.Equals(nombreVO.Valor, gender.Nombre.Valor, StringComparison.OrdinalIgnoreCase) &&
            await _repository.ExistsByNombreAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro género con el nombre '{nombreVO.Valor}'.");

        gender.ActualizarNombre(nombreVO.Valor);
        await _repository.UpdateAsync(gender);
        return gender;
    }
}
