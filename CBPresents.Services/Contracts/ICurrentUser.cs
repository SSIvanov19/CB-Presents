namespace CBPresents.Services.Contracts;

public interface ICurrentUser
{
    string UserEmail { get; }

    string Role { get; }
}
