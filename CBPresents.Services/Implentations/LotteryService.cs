using AutoMapper;
using CBPresents.Data.Data;
using CBPresents.Data.Models;
using CBPresents.Services.Contracts;
using CBPresents.Shared.Models;

namespace CBPresents.Services.Implentations;

public class LotteryService : ILotteryService
{
    private readonly ApplicationDBContext context;
    private readonly IMapper mapper; 

    public LotteryService(ApplicationDBContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<LotteryEntryVM> Participate(string userEmail)
    {
        var user = this.context.LotteryEntries.FirstOrDefault(e => e.Email == userEmail);

        if (user != null)
        {
            return new LotteryEntryVM()
            {
                AlreadyParticipated = true
            };
        }

        LotteryEntry lotteryEntry = new()
        {
            Id = Guid.NewGuid().ToString(),
            Email = userEmail,
            IsWinner = false,
        };

        await this.context.LotteryEntries.AddAsync(lotteryEntry);

        await this.context.SaveChangesAsync();

        return this.mapper.Map<LotteryEntryVM>(lotteryEntry);
    }
}
