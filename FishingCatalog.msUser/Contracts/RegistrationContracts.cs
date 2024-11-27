namespace FishingCatalog.msUser.Contracts
{
    public record RegistrationRequest(
            string Name,
            string Email,
            string Password
            );
}
