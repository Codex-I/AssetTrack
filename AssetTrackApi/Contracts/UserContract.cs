using AssetTrackApi.Models;

namespace AssetTrackApi.Contracts
{
    public class CreateUserRequest
    {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Password { get; init; }
        public required string Email { get; init; }
        public required string PhoneNumber { get; init; }
    }

    public class UpdateUserRequest
    {
        public required string Email { get; init; }
        public required string PhoneNumber { get; init; }
    }

    public class UpdateUserPasswordRequest
    {
        public required string OldPassword { get; init; }
        public required string NewPassword { get; init; }
    }

    public class UserResponse(User user)
    {
        public string UserId { get; } = user.Id;
        public string Fullname { get; } = user.FullName;
        public string Email { get; } = user.Email;
        public string PhoneNumber { get; } = user.PhoneNumber;
        public DateTime DateCreated { get; } = user.DateCreated;
    }
}
