using AuthDemo.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AuthDemo.Models
{
    public class Rol : IdentityRole<int>
    {
        public enuPermissions Permissions { get; set; }
    }
}
