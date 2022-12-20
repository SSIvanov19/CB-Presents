namespace CBPresents.Shared.Models;

public class LotteryEntryVM
{
    public string? Id { get; set; }
    
    public DateTime LotteryTime { get; set; }

    public string? Email { get; set; }

    public bool? IsWinner { get; set; }

    public bool? AlreadyParticipated { get; set; }

    public bool? CanParticipate { get; set; }

}
