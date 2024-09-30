using AssetTrackApi.Contracts;
using AssetTrackApi.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetTrackApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(ControllerParameters services) : PublicControllerBase(services)
    {
        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult<UserLoginResponse>> Login([FromBody] UserLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await Context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !user.VerifyPassword(request.Password))
                return Unauthorized(new { message = "Invalid username or password" });

            var jwtResponse = GenerateJwtToken(user);

            await Context.SaveChangesAsync();


            return Ok(new UserLoginResponse(jwtResponse.Token, user.Id, user.Email, jwtResponse.Expires));
        }

        [Authorize]
        [HttpDelete("current")]
        public ActionResult Logout()
        {
            //destroy the token
            return Ok();
        }
    }
}
