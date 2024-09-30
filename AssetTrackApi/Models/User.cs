using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace AssetTrackApi.Models
{
    public class User
    {
        public User() { }

        public User(string firstName, string lastName, string email, string password, string phoneNumber)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);
            ArgumentException.ThrowIfNullOrWhiteSpace(phoneNumber);

            Id = email;
            FullName = firstName + lastName;
            Email = email;
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            PhoneNumber = phoneNumber;
            DateCreated = DateTime.UtcNow;
        }

        [Key]
        public string Id { get; private set; }
        public string FullName { get; private set; }  // User's full name
        public string Email { get; private set; }  // User's email address
        public string PhoneNumber { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime DateCreated { get; private set; }

        public virtual ICollection<Asset> ManagedAssets { get; private set; }

        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }

        public void UpdateUserInfo(string email, string phoneNumber)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(phoneNumber);

            Email = email;
            PhoneNumber = phoneNumber;
        }

        public void UpdateUserPassword(string newPassword)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newPassword);
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        }

        public void AddAsset(Asset asset)
        {
            ArgumentNullException.ThrowIfNull(asset);
            ManagedAssets.Add(asset);
        }

        public void RemoveAsset(Asset asset)
        {
            ArgumentNullException.ThrowIfNull(asset);
            ManagedAssets.Remove(asset);
        }
    }
}
