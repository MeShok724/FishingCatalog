namespace FishingCatalog.msUser.Contracts
{
    public record AuthorizationRequest(
        string Email,
        string Password
        );
}
