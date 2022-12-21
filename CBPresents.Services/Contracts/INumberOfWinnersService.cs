namespace CBPresents.Services.Contracts;

public interface INumberOfWinnersService
{
    Task SetNumberOfWiinners(int number);

    Task SetExplicitNumberOfWiinners(int time);

    Task<int?> GetNumberOfWinners();
}
