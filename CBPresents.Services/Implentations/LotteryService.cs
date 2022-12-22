using AutoMapper;
using AutoMapper.QueryableExtensions;
using CBPresents.Data.Data;
using CBPresents.Data.Models;
using CBPresents.Services.Contracts;
using CBPresents.Shared.Models;
using Microsoft.EntityFrameworkCore;

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

    public async Task<LotteryEntryVM> CheckIfWinner(string userEmail)
    {
        var user = await this.context
            .LotteryEntries
            .FirstOrDefaultAsync(e => e.Email == userEmail);

        return new LotteryEntryVM()
        {
            IsWinner = user.IsWinner ?? false
        };
    }

    public async Task<List<User>> GetParticipingUsers()
    {
        return await this.context
            .LotteryEntries
            .ProjectTo<User>(this.mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<LotteryEntryVM> Participate(string userEmail, string name)
    {
        var user = this.context
            .LotteryEntries
            .FirstOrDefault(e => e.Email == userEmail);

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
            Name = name,
            IsWinner = false,
        };

        await this.context.LotteryEntries.AddAsync(lotteryEntry);

        await this.context.SaveChangesAsync();

        return this.mapper.Map<LotteryEntryVM>(lotteryEntry);
    }

    public async Task PickWinners(int numberOfWinners)
    {
        // Pick random winners

        var lotteryEntries = await this.context
            .LotteryEntries
            .ToListAsync();

        var winners = new List<LotteryEntry>();

        for (int i = 0; i < numberOfWinners; i++)
        {
            var random = new Random();
            var winner = lotteryEntries[random.Next(0, lotteryEntries.Count)];

            winners.Add(winner);
        }

        // Update the database

        foreach (var winner in winners)
        {
            winner.IsWinner = true;
        }

        this.context.LotteryEntries.UpdateRange(winners);

        await this.context.SaveChangesAsync();

        return;
    }
}
