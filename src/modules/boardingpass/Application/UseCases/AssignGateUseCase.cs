// src/modules/boardingpass/Application/UseCases/AssignGateUseCase.cs
using AirTicketSystem.modules.boardingpass.Domain.aggregate;
using AirTicketSystem.modules.boardingpass.Domain.Repositories;

namespace AirTicketSystem.modules.boardingpass.Application.UseCases;

public sealed class AssignGateUseCase
{
    private readonly IBoardingPassRepository _repository;

    public AssignGateUseCase(IBoardingPassRepository repository) => _repository = repository;

    public async Task<BoardingPass> ExecuteAsync(
        int id, int puertaEmbarqueId, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del pase de abordar no es válido.");

        var boardingPass = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un pase de abordar con ID {id}.");

        boardingPass.AsignarPuerta(puertaEmbarqueId);
        await _repository.UpdateAsync(boardingPass);
        return boardingPass;
    }
}
