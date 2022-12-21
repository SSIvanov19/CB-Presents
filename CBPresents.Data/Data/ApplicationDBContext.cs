using CBPresents.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CBPresents.Data.Data;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LotteryEntry> LotteryEntries { get; set; }

    public virtual DbSet<LotteryTime> LotteryTimes { get; set; }

    public virtual DbSet<NumberOfWinners> NumberOfWinners { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }
}
