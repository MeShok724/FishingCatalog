namespace FishinfCatalog.msAuthorization
{
    public class PasswordHasher
    {
        public static string Generate(string password) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        public static bool Verify(string password, string hash) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}
