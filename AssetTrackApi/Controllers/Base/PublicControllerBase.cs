using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using AssetTrackApi.Data;
using AssetTrackApi.Contracts;
using AssetTrackApi.Models;

namespace AssetTrackApi.Controllers.Base
{
    [ApiController]
    [Produces("application/json")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public abstract class PublicControllerBase(ControllerParameters services) : ControllerBase
    {
        protected AssetTrackContext Context { get; } = services.DbContext;

        protected JwtResponse GenerateJwtToken(User user)
        {
            ArgumentNullException.ThrowIfNull(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(services.Settings.JWT.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                ]),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = services.Settings.JWT.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new JwtResponse
            {
                Token = tokenHandler.WriteToken(token),
                Expires = tokenDescriptor.Expires
            };
        }

    }
}
