namespace FishingCatalog.msCatalog.Contracts
{
    public record ProductResponse(
    Guid Id,
    string Name,
    decimal Price,
    string Category,
    string Description,
    byte[] Image);

    public record ProductRequest
    (
    string Name,
    decimal Price,
    string Category,
    string Description,
    byte[] Image);
}
