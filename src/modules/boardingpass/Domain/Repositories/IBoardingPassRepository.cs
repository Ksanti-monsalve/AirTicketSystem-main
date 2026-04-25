// src/modules/boardingpass/Domain/Repositories/IBoardingPassRepository.cs
using AirTicketSystem.modules.boardingpass.Domain.aggregate;

namespace AirTicketSystem.modules.boardingpass.Domain.Repositories;

public interface IBoardingPassRepository
{
    Task<BoardingPass?> FindByIdAsync(int id);
    Task<BoardingPass?> FindByCodigoPaseAsync(string codigoPase);
    Task<BoardingPass?> FindByCheckinAsync(int checkinId);
    Task<IReadOnlyCollection<BoardingPass>> FindByPuertaAsync(int puertaEmbarqueId);
    Task<bool> ExistsByCodigoPaseAsync(string codigoPase);
    Task<bool> ExistsByCheckinAsync(int checkinId);
    Task SaveAsync(BoardingPass boardingPass);
    Task UpdateAsync(BoardingPass boardingPass);
}
