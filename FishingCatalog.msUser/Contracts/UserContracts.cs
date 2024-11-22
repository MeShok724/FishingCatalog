namespace FishingCatalog.msUser.Contracts
{
    public record UserResponse(
        Guid Id,
        string Name,
        string Email,
        string PasswordHash,
        DateTime CreatedAt,
        DateTime LastLogin,
        bool IsActive,
        Guid RoleId
        );
    public record UserToAdd(
        string Name,
        string Email,
        string PasswordHash
        );
    public record UserToUpdate(
        string Name,
        string Email,
        string PasswordHash,
        DateTime CreatedAt,
        DateTime LastLogin,
        bool IsActive,
        Guid RoleId
        );

}