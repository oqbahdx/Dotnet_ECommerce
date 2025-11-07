using System.Text.RegularExpressions;

namespace ECommerce.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        // Personal Info
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public string? ProfileImageUrl { get; private set; }

        //  Authentication
        public string PasswordHash { get; private set; } = string.Empty;
        public bool IsEmailVerified { get; private set; }
        public bool IsActive { get; private set; } = true;

        //  Role (Admin / Customer / Seller)
        public string Role { get; private set; } = "Customer";

        //  Auditing
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; }

        //  Navigation properties (Relations)
        public ICollection<Address> Addresses { get; private set; } = new List<Address>();
        public ICollection<Order> Orders { get; private set; } = new List<Order>();

        //  Empty constructor for EF
        private User() { }

        public User(string firstName, string lastName, string email, string passwordHash, string phoneNumber)
        {
            SetName(firstName, lastName);
            SetEmail(email);
            SetPhone(phoneNumber);
            SetPasswordHash(passwordHash);
        }

        //  Domain validation methods
        private void SetName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.");

            if (firstName.Length > 100 || lastName.Length > 100)
                throw new ArgumentException("First and Last names must not exceed 100 characters.");

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
        }

        private void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");

            //  Simple but strong email validation
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, emailRegex, RegexOptions.IgnoreCase))
                throw new ArgumentException("Invalid email format.");

            if (email.Length > 256)
                throw new ArgumentException("Email must not exceed 256 characters.");

            Email = email.Trim().ToLowerInvariant();
        }

        private void SetPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Phone number is required.");

            //  Basic numeric validation (you can adjust this for country codes)
            var phoneRegex = @"^[0-9+\-\s]{6,20}$";
            if (!Regex.IsMatch(phone, phoneRegex))
                throw new ArgumentException("Invalid phone number format.");

            PhoneNumber = phone.Trim();
        }

        public void SetPasswordHash(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException("Password hash cannot be empty.");

            PasswordHash = hash;
            UpdatedAt = DateTime.UtcNow;
        }

        // Domain Behaviors
        public void VerifyEmail()
        {
            IsEmailVerified = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateProfile(string? firstName, string? lastName, string? phone, string? profileImage)
        {
            if (!string.IsNullOrWhiteSpace(firstName)) FirstName = firstName;
            if (!string.IsNullOrWhiteSpace(lastName)) LastName = lastName;
            if (!string.IsNullOrWhiteSpace(phone)) SetPhone(phone);
            if (!string.IsNullOrWhiteSpace(profileImage)) ProfileImageUrl = profileImage;

            UpdatedAt = DateTime.UtcNow;
        }
    }
}
