namespace BetAPI.Services
{
    public interface IAuthenticationService
    {
        Task<bool> LoginAsync(string username, string password);
    }
}
