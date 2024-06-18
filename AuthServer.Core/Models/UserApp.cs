using Microsoft.AspNetCore.Identity;

namespace AuthServer.Core.Models;

public class UserApp : IdentityUser<Guid>
{
    public string City { get; set; }
}
