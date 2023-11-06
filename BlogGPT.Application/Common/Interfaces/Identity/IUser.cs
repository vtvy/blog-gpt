namespace BlogGPT.Application.Common.Interfaces.Identity
{
    public interface IUser
    {
        string? Id { get; }
        string? UserName { get; }
    }
}
