using Microsoft.AspNetCore.Identity;

namespace Casino_Royale_Backend.Entities;

public class User : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}
