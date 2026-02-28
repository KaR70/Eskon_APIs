using Microsoft.AspNetCore.Identity;

namespace Eskon.Domain.Entities;

public sealed class ApplicationRole : IdentityRole
{
    public bool IsDefault { get; set; }
    public bool IsDisabled { get; set; }
}
