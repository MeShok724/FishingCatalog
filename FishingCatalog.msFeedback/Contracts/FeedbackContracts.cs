namespace FishingCatalog.msFeedback.Contracts
{
    public record FeedbackRequest(
        Guid UserId,
        Guid ProductId,
        string? Comment);
}
