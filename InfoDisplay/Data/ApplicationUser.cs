using Microsoft.AspNetCore.Identity;

namespace InfoDisplay.Data;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}