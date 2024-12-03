using FishingCatalog.Core;

namespace FishingCatalog.msUser.Infrastructure
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}