using CBPresents.Data.Data;
using CBPresents.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBPresents.Services.Implentations;

public class TimeService : ITimeService
{
    private readonly ApplicationDBContext context;
    
    public TimeService(ApplicationDBContext context)
    {
        this.context = context;        
    }

    public async Task<DateTime?> GetTime()
    {
        return (await this.context.LotteryTimes.FirstOrDefaultAsync())!.Time;
    }

    public async Task SetTime(string time)
    {
        var lotteryTime = await this.context.LotteryTimes.FirstOrDefaultAsync();

        if (lotteryTime != null)
        {
            return;
        }

        await this.context.LotteryTimes.AddAsync(
            new Data.Models.LotteryTime()
            {
                Id = Guid.NewGuid().ToString(),
                Time = DateTime.Parse(time, null, System.Globalization.DateTimeStyles.RoundtripKind)
            });

        await this.context.SaveChangesAsync();

        return;
    }
}
