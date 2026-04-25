// src/shared/helpers/PasswordHasher.cs
using System.Security.Cryptography;
using System.Text;

namespace AirTicketSystem.shared.helpers;

public static class PasswordHasher
{
    public static string Hash(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    public static bool Verify(string password, string hash)
        => Hash(password) == hash;
}
