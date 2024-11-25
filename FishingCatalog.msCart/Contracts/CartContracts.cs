namespace FishingCatalog.msCart.Contracts
{
    public record CartRequest(
        Guid UserId,
        Guid ProductId,
        int Amount
        );
}
