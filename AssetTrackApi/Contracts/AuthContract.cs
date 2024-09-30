namespace AssetTrackApi.Contracts
{
    public class UserLoginRequest
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }

    public class JwtResponse
    {
        public string Token { get; set; }
        public DateTime? Expires { get; set; }
    }

    public class UserLoginResponse(string accessToken, string userId, string email, DateTime? expiresIn)
    {
        public string AccessToken { get; } = accessToken;
        public string TokenType { get; } = "Bearer";
        public string UserId { get; }  = userId;
        public string Email { get; } = email;
        public DateTime? TokenExpiresAt { get; } = expiresIn;
    }
}
