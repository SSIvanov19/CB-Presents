using CBPresents.Data.Data;
using CBPresents.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CBPresents.Services.Implentations;

public class NumberOfWinnersService : INumberOfWinnersService
{
    private readonly ApplicationDBContext context;
    
    public NumberOfWinnersService(ApplicationDBContext context)
    {
        this.context = context;
    }

    public async Task<int?> GetNumberOfWinners()
    {
        return (await this.context.NumberOfWinners.FirstOrDefaultAsync())!.Winners;
    }

    public async Task SetExplicitNumberOfWiinners(int number)
    {
        this.context.NumberOfWinners.RemoveRange(this.context.NumberOfWinners);

        this.context.NumberOfWinners.Add(new()
        {
            Id = Guid.NewGuid().ToString(),
            Winners = number
        });

        await this.context.SaveChangesAsync();

        return;
    }

    public async Task SetNumberOfWiinners(int number)
    {
        var numberOfWinners = await this.context.NumberOfWinners.FirstOrDefaultAsync();

        if (numberOfWinners != null)
        {
            return;
        }

        await this.context.NumberOfWinners.AddAsync(
            new Data.Models.NumberOfWinners()
            {
                Id = Guid.NewGuid().ToString(),
                Winners = number
            });

        await this.context.SaveChangesAsync();

        return;
    }
}
