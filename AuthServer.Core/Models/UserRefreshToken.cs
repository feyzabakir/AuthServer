namespace AuthServer.Core.Models;

public class UserRefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Code { get; set; }
    public DateTimeOffset Expiration { get; set; }
    public UserApp User { get; set; }
}
