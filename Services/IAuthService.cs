namespace Skill.Integration.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user with the given username and password.
        /// </summary>
        /// <param name="username">The username for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <returns>A JWT token for the newly registered user.</returns>
        Task<string> RegisterAsync(string username, string password);

        /// <summary>
        /// Authenticates a user with the given username and password.
        /// </summary>
        /// <param name="username">The username of the user trying to log in.</param>
        /// <param name="password">The password of the user trying to log in.</param>
        /// <returns>A JWT token for the authenticated user.</returns>
        Task<string> LoginAsync(string username, string password);
    }
}
