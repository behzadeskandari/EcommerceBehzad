namespace EcommerceBehzad.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public string Salt { get; private set; } = null!;
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private readonly List<RefreshToken> _refreshTokens = new();
        public virtual IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        private User() { } // EF Core Required

        public User(string email, string passwordHash, string salt, string firstName, string lastName)
        {
            Id = Guid.NewGuid();
            Email = email.ToLowerInvariant().Trim();
            PasswordHash = passwordHash;
            Salt = salt;
            FirstName = firstName;
            LastName = lastName;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        public void AddRefreshToken(string token, DateTime expiresAt)
        {
            _refreshTokens.Add(new RefreshToken(token, expiresAt, Id));
        }

        public void RevokeToken(string token)
        {
            var existingToken = _refreshTokens.FirstOrDefault(t => t.Token == token);
            existingToken?.Revoke();
        }
    }
}
