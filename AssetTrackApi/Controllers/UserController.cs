using AssetTrackApi.Contracts;
using AssetTrackApi.Contracts.QueryHelpers;
using AssetTrackApi.Controllers.Base;
using AssetTrackApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AssetTrackApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(ControllerParameters services) : SecureControllerBase(services)
    {
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (Context.Users.Any(u => u.Id == request.Email))
            {
                return BadRequest(new { message = "User already exists." });
            }

            var user = new User(request.FirstName, request.LastName, request.Email, request.Password, request.PhoneNumber);

            Context.Users.Add(user);
            await Context.SaveChangesAsync();

            return Ok(new UserResponse(user));
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> UsersList([FromQuery] PaginationQuery paging, [FromQuery] string? searchText)
        {
            IQueryable<User> query = Context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(a => a.FullName.Contains(searchText));
            }

            return Ok(await ToPagedListAsync(query, paging, x => new UserResponse(x)));
        }

        [HttpGet]
        public async Task<ActionResult> Profile()
        {
            var user = await Context.Users
                .Include(u => u.ManagedAssets)
                .FirstOrDefaultAsync(u => u.Id == CurrentUserId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(new UserResponse(user));
        }

        [HttpPut("update")]
        public async Task<ActionResult> Update([FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await Context.Users.FindAsync(CurrentUserId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            user.UpdateUserInfo(request.Email, request.PhoneNumber);

            Context.Users.Update(user);
            await Context.SaveChangesAsync();

            return Ok(new UserResponse(user));
        }

        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdateUserPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Fetch the user's ID from the claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User ID claim not found" });

            // Find the user in the database using the ID from the claim
            var user = await Context.Users.FindAsync(userId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            // Verify the current password
            if (!user.VerifyPassword(request.OldPassword))
                return Unauthorized(new { message = "Invalid current password" });

            // Update the user's password
            user.UpdateUserPassword(request.NewPassword);

            Context.Users.Update(user);
            await Context.SaveChangesAsync();

            return Ok(new { message = "Password updated successfully" });
        }

    }
}
