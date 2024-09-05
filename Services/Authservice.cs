using Microsoft.IdentityModel.Tokens;
using Skill.Integration.Models;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Skill.Integration.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        // Thread-safe dictionary to store users in memory
        private static readonly ConcurrentDictionary<string, User> _users = new ConcurrentDictionary<string, User>();

        // Counter for generating unique user IDs
        private static int _nextId = 1;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Registers a new user and returns a JWT token.
        /// </summary>
        /// <param name="username">The username for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <returns>A JWT token for the newly registered user.</returns>
        /// <exception cref="Exception">Thrown when registration fails.</exception>
        public async Task<string> RegisterAsync(string username, string password)
        {
            // Check if the username already exists
            if (_users.ContainsKey(username))
            {
                throw new Exception("Username already exists");
            }

            // Hash the password for secure storage
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // Create a new user with a unique ID
            var user = new User { Id = Interlocked.Increment(ref _nextId), Username = username, PasswordHash = passwordHash };

            // Try to add the user to the dictionary
            if (_users.TryAdd(username, user))
            {
                return GenerateJwtToken(user);
            }

            throw new Exception("Failed to register user");
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="username">The username of the user trying to log in.</param>
        /// <param name="password">The password of the user trying to log in.</param>
        /// <returns>A JWT token for the authenticated user.</returns>
        /// <exception cref="Exception">Thrown when authentication fails.</exception>
        public async Task<string> LoginAsync(string username, string password)
        {
            // Try to get the user from the dictionary and verify the password
            if (_users.TryGetValue(username, out var user) && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return GenerateJwtToken(user);
            }

            throw new Exception("Invalid username or password");
        }

        /// <summary>
        /// Generates a JWT token for the given user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <returns>A JWT token string.</returns>
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            }),
                Expires = DateTime.UtcNow.AddDays(7), // Token expires in 7 days
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
