using Eskon.Domain.Abstraction.Consts;
using Microsoft.AspNetCore.Identity;

namespace Eskon.Infrastructure.Persistance.EntitiesConfigurations;
public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        //Default Data
        builder.HasData(new IdentityUserRole<string>
        {
            UserId = DefaultUsers.AdminId,
            RoleId = DefaultRoles.AdminRoleId
        });
    }
}