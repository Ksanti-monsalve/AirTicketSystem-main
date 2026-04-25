// src/modules/boardingpass/Application/UseCases/GetBoardingPassByCheckInUseCase.cs
using AirTicketSystem.modules.boardingpass.Domain.aggregate;
using AirTicketSystem.modules.boardingpass.Domain.Repositories;

namespace AirTicketSystem.modules.boardingpass.Application.UseCases;

public sealed class GetBoardingPassByCheckInUseCase
{
    private readonly IBoardingPassRepository _repository;

    public GetBoardingPassByCheckInUseCase(IBoardingPassRepository repository)
        => _repository = repository;

    public async Task<BoardingPass> ExecuteAsync(
        int checkinId,
        CancellationToken cancellationToken = default)
    {
        if (checkinId <= 0)
            throw new ArgumentException("El ID del check-in no es válido.");

        return await _repository.FindByCheckinAsync(checkinId)
            ?? throw new KeyNotFoundException(
                "No hay pase de abordar para este check-in. " +
                "Debe generarse después del check-in.");
    }
}

