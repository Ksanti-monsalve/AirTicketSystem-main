// src/modules/boardingpass/Application/UseCases/AssignBoardingTimeUseCase.cs
using AirTicketSystem.modules.boardingpass.Domain.aggregate;
using AirTicketSystem.modules.boardingpass.Domain.Repositories;

namespace AirTicketSystem.modules.boardingpass.Application.UseCases;

public sealed class AssignBoardingTimeUseCase
{
    private readonly IBoardingPassRepository _repository;

    public AssignBoardingTimeUseCase(IBoardingPassRepository repository) => _repository = repository;

    public async Task<BoardingPass> ExecuteAsync(
        int id,
        DateTime horaEmbarque,
        DateTime fechaSalidaVuelo,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del pase de abordar no es válido.");

        var boardingPass = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un pase de abordar con ID {id}.");

        boardingPass.AsignarHoraEmbarque(horaEmbarque, fechaSalidaVuelo);
        await _repository.UpdateAsync(boardingPass);
        return boardingPass;
    }
}
