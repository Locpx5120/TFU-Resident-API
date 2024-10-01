namespace Core.Infrastructure
{
    public interface IUserIdentity
    {
        Guid? UserId { get; }
        string? UserName { get; }
        string? RoleName { get; }
    }
}
