using System.Data.Entity;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Website.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("Name", this.Name ?? ""));
            userIdentity.AddClaim(new Claim("Id", this.Id ?? ""));
            return userIdentity;
        }
    }

    public static class IdentityExtensions
    {
        public static ApplicationUser GetApplicationUser(this IIdentity identity)
        {
            var claimName = ((ClaimsIdentity)identity).FindFirst("Name");
            //var userIdClaim = ((ClaimsIdentity)identity).Claims.
            //    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            //if (userIdClaim != null)
            //{
            //    var userIdValue = userIdClaim.Value;
            //}

            var claimId = ((ClaimsIdentity)identity).FindFirst("Id");
            // Test for null to avoid issues during local testing
            var user = new ApplicationUser
            {
                Name = string.IsNullOrWhiteSpace(claimName?.Value) ? null : claimName.Value,  // != null ? claimName.Value : string.Empty
                //Id = claimId.Value  // != null ? claimName.Value : string.Empty
            };
            return user;
        }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}