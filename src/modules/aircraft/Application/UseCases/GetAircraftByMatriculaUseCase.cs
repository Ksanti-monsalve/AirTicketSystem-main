// src/modules/aircraft/Application/UseCases/GetAircraftByMatriculaUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Domain.aggregate;
using AirTicketSystem.modules.aircraft.Domain.ValueObjects;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class GetAircraftByMatriculaUseCase
{
    private readonly IAircraftRepository _repository;

    public GetAircraftByMatriculaUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task<Aircraft> ExecuteAsync(
        string matricula,
        CancellationToken cancellationToken = default)
    {
        var matriculaVO = MatriculaAircraft.Crear(matricula);

        return await _repository.FindByMatriculaAsync(matriculaVO.Valor)
            ?? throw new KeyNotFoundException(
                $"No se encontró un avión con matrícula '{matriculaVO.Valor}'.");
    }
}