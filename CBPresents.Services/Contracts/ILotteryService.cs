
using CBPresents.Data.Models;
using CBPresents.Shared.Models;

namespace CBPresents.Services.Contracts;

public interface ILotteryService
{
    Task<LotteryEntryVM> Participate(string userEmail);
}
