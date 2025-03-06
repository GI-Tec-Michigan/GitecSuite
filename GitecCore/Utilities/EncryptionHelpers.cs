using System.Security.Cryptography;
using System.Text;

namespace Gitec.Utilities;

public static class EncryptionHelpers
{
    public static string HashString(string input)
    {
        using var sha256 = SHA256.Create();
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = sha256.ComputeHash(inputBytes);
            
        return Convert.ToHexString(hashBytes); // Converts to uppercase hex string
    }
    
    public static bool VerifyHashedString(string PasswordHash, string PasswordClearText)
    {
        return PasswordHash == HashString(PasswordClearText);
    }
}