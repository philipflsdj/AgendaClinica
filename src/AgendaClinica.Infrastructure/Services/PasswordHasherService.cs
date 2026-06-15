using AgendaClinica.Domain.Contracts.Services;
using Microsoft.AspNetCore.Identity;

namespace AgendaClinica.Infrastructure.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly PasswordHasher<object> _hasher = new();

        public string HashPassword(string password)
            => _hasher.HashPassword(new object(), password);

        public bool VerifyPassword(string password, string hash)
            => _hasher.VerifyHashedPassword(new object(), hash, password) != PasswordVerificationResult.Failed;
    }
}
