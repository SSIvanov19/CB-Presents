
using CBPresents.Data.Models;
using CBPresents.Shared.Models;

namespace CBPresents.Services.Contracts;

public interface ILotteryService
{
    Task<LotteryEntryVM> Participate(string userEmail, string name);

    Task<LotteryEntryVM> CheckIfWinner(string userEmail);

    Task PickWinners(int numberOfWinners);

    Task<List<User>> GetParticipingUsers();
}
