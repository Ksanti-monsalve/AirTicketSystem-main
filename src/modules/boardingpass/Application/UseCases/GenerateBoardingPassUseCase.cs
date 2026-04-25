// src/modules/boardingpass/Application/UseCases/GenerateBoardingPassUseCase.cs
using AirTicketSystem.modules.boardingpass.Domain.aggregate;
using AirTicketSystem.modules.boardingpass.Domain.Repositories;
using AirTicketSystem.modules.checkin.Domain.Repositories;

namespace AirTicketSystem.modules.boardingpass.Application.UseCases;

public sealed class GenerateBoardingPassUseCase
{
    private readonly IBoardingPassRepository _boardingPassRepository;
    private readonly ICheckInRepository      _checkInRepository;

    public GenerateBoardingPassUseCase(
        IBoardingPassRepository boardingPassRepository,
        ICheckInRepository      checkInRepository)
    {
        _boardingPassRepository = boardingPassRepository;
        _checkInRepository      = checkInRepository;
    }

    public async Task<BoardingPass> ExecuteAsync(
        int checkinId,
        string numeroVuelo,
        string codigoAsiento,
        DateTime fechaSalidaVuelo,
        int? puertaEmbarqueId = null,
        DateTime? horaEmbarque = null,
        CancellationToken cancellationToken = default)
    {
        _ = await _checkInRepository.FindByIdAsync(checkinId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un check-in con ID {checkinId}.");

        if (await _boardingPassRepository.ExistsByCheckinAsync(checkinId))
            throw new InvalidOperationException(
                "Ya existe un pase de abordar para este check-in.");

        var boardingPass = BoardingPass.Crear(
            checkinId,
            numeroVuelo,
            codigoAsiento,
            fechaSalidaVuelo,
            puertaEmbarqueId,
            horaEmbarque);

        await _boardingPassRepository.SaveAsync(boardingPass);
        return boardingPass;
    }
}
