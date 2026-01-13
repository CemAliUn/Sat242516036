using Microsoft.AspNetCore.Identity;

namespace Sat242516036.Data;

public class ApplicationUser : IdentityUser
{
    // SQL tablosunda [FullName] nvarchar(max) tanýmlamýþtýk, karþýlýðý bu:
    public string? FullName { get; set; }
}