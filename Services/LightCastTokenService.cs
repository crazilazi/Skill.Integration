﻿using Flurl.Http;
using Skill.Integration.Models;
using System.Text.Json;

namespace Skill.Integration.Services
{
    public class LightCastTokenService : ILightCastTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly Dictionary<string, LightCastAccessToken> _tokenCache = [];
        private readonly object _lock = new();
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public LightCastTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Get a valid token
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public async Task<string> GetValidTokenAsync(string scope)
        {
            // Fast path - if token exists and is valid for the requested scope, return it
            if (_tokenCache.TryGetValue(scope, out var cachedToken) && cachedToken.IsValid())
            {
                return cachedToken.AccessToken;
            }

            // Slow path - token needs to be generated
            lock (_lock)
            {
                // Double-check the token hasn't been generated by another thread while we were waiting
                if (_tokenCache.TryGetValue(scope, out cachedToken) && cachedToken.IsValid())
                {
                    return cachedToken.AccessToken;
                }

                // Generate new token synchronously since we're already in a lock
                return GenerateNewTokenAsync(scope).GetAwaiter().GetResult().AccessToken;
            }
        }

        /// <summary>
        /// Generate a new token
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<LightCastAccessToken> GenerateNewTokenAsync(string scope)
        {
            var clientId = _configuration["LightCast:ClientId"];
            var clientSecret = _configuration["LightCast:ClientSecret"];

            try
            {
                var response = await "https://auth.emsicloud.com/connect/token"
                    .WithHeader("Content-Type", "application/x-www-form-urlencoded")
                    .PostUrlEncodedAsync(new
                    {
                        client_id = clientId,
                        client_secret = clientSecret,
                        grant_type = "client_credentials",
                        scope
                    });

                string content = await response.GetStringAsync();

                var newToken = JsonSerializer.Deserialize<LightCastAccessToken>(content, _jsonOptions) ?? throw new Exception("Failed to deserialize token response");

                // Store the new token in the cache
                _tokenCache[scope] = newToken;

                return newToken;
            }
            catch (FlurlHttpException ex)
            {
                var errorMessage = await ex.GetResponseStringAsync();
                throw new Exception($"Failed to generate token: {errorMessage}", ex);
            }
        }
    }
}
