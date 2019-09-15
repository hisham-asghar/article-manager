using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LayerDb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Website.Models;

namespace Website.Models.MyStores
{
    public class MyUserStore : IUserStore<ApplicationUser>, 
        IUserPasswordStore<ApplicationUser>, IUserSecurityStampStore<ApplicationUser>,
        IUserRoleStore<ApplicationUser>,
        IUserLoginStore<ApplicationUser>, IUserClaimStore<ApplicationUser>,
        IUserLockoutStore<ApplicationUser,string>,IUserTwoFactorStore<ApplicationUser,string>,
        IUserEmailStore<ApplicationUser,string>

    {
       // UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
        public MyUserStore()
        {
        }
        public Task CreateAsync(ApplicationUser user)
        {
            bool status = LayerBao.UserBao.Create(ToAspUser(user));
            return Task.Run(()=> {});
        }
        public Task DeleteAsync(ApplicationUser user)
        {
            return Task.Run(() => LayerBao.UserBao.RemoveUserById(user.Id));
            //var context = userStore.Context as ApplicationDbContext;
            //context.Users.Remove(user);
            //context.Configuration.ValidateOnSaveEnabled = false;
            //return context.SaveChangesAsync();
        }
        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            var aspUser = LayerBao.UserBao.GetAspUser(null, userId);
            return Task.Run(() => ToApplicationUser(aspUser));
            //var context = userStore.Context as ApplicationDbContext;
            //return context.Users.Where(u => u.Id.ToLower() == userId.ToLower()).FirstOrDefaultAsync();
        }
        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var aspUser = LayerBao.UserBao.GetAspUser(userName, null);
            return Task.Run(() => ToApplicationUser(aspUser));
            //var context = userStore.Context as ApplicationDbContext;
            //return context.Users.Where(u => u.UserName.ToLower() == userName.ToLower()).FirstOrDefaultAsync();
        }
        public Task UpdateAsync(ApplicationUser user)
        {
            return Task.Run(() => LayerBao.UserBao.Update(ToAspUser(user), user.Id));

            //var context = userStore.Context as ApplicationDbContext;
            //context.Users.Attach(user);
            //context.Entry(user).State = EntityState.Modified;
            //context.Configuration.ValidateOnSaveEnabled = false;
            //return context.SaveChangesAsync();
        }
        public void Dispose()
        {
           // userStore.Dispose();
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            var identityUser = ToIdentityUser(user);
            //var task = userStore.GetPasswordHashAsync(identityUser);
            //SetApplicationUser(user, identityUser);
            //return task;


            var aspUser = LayerBao.UserBao.GetAspUser(user.Email, user.Id);
            user.PasswordHash = aspUser.PasswordHash;
            SetApplicationUser(user, identityUser);
            return Task.Run(() => aspUser.PasswordHash);

        }
        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            var identityUser = ToIdentityUser(user);
            //var task = userStore.HasPasswordAsync(identityUser);
            var aspUser = LayerBao.UserBao.GetAspUser(user.Email, user.Id);
            user.PasswordHash = aspUser.PasswordHash;
            SetApplicationUser(user, identityUser);
            return Task.Run(() => aspUser.PasswordHash != null);
            //return task;
        }
        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            var identityUser = ToIdentityUser(user);
            //var task = userStore.SetPasswordHashAsync(identityUser, passwordHash);
            var aspUser = LayerBao.UserBao.GetAspUser(user.Email, user.Id);
            aspUser.PasswordHash = passwordHash;
            identityUser.PasswordHash = passwordHash;
            LayerBao.UserBao.Update(aspUser,user.Id);
            return Task.Run(() => SetApplicationUser(user, identityUser));


            //return task;
        }
        public Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            var identityUser = ToIdentityUser(user);
            //var task = userStore.GetSecurityStampAsync(identityUser);
            var aspUser = LayerBao.UserBao.GetAspUser(user.Email, user.Id);
            user.SecurityStamp = aspUser.SecurityStamp;
            SetApplicationUser(user, identityUser);
            return Task.Run(() => aspUser.SecurityStamp);
        }
        public Task SetSecurityStampAsync(ApplicationUser user, string stamp)
        {
            var identityUser = ToIdentityUser(user);
            //var task = userStore.SetSecurityStampAsync(identityUser, stamp);
            //SetApplicationUser(user, identityUser);


            var aspUser = LayerBao.UserBao.GetAspUser(user.Email, user.Id);
            user.SecurityStamp = aspUser.SecurityStamp;
            
            return Task.Run(() => SetApplicationUser(user, identityUser));



            //return task;
        }
        private static void SetApplicationUser(ApplicationUser user, IdentityUser identityUser)
        {
            user.PasswordHash = identityUser.PasswordHash;
            user.SecurityStamp = identityUser.SecurityStamp;
            user.Id = identityUser.Id;
            user.UserName = identityUser.UserName;
        }
        private static IdentityUser ToIdentityUser(ApplicationUser user)
        {
            return new IdentityUser
            {
                Id = user.Id,
                PasswordHash = user.PasswordHash,
                SecurityStamp = user.SecurityStamp,
                UserName = user.UserName
            };
        }

        public static ApplicationUser ToApplicationUser(AspNetUser user)
        {
            if (user == null) return null;
            return new ApplicationUser()
            {
                Id = user.Id,
                PasswordHash = user.PasswordHash,
                SecurityStamp = user.SecurityStamp,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AccessFailedCount = user.AccessFailedCount,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEndDateUtc = user.LockoutEndDateUtc,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                Name = user.Name
            };
        }

        public static AspNetUser ToAspUser(ApplicationUser user)
        {
            return new AspNetUser()
            {
                Id = user.Id,
                PasswordHash = user.PasswordHash,
                SecurityStamp = user.SecurityStamp,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AccessFailedCount = user.AccessFailedCount,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEndDateUtc = user.LockoutEndDateUtc,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled
            };
        }

        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            var userLogin = new AspNetUserLogin()
            {
                UserId = user.Id,
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            };
            return Task.Run(() => LayerBao.UserBao.AddExternalLogin(userLogin));
        }

        public Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            var userLogin = new AspNetUserLogin()
            {
                UserId = user.Id,
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            };
            return Task.Run(() => LayerBao.UserBao.RemoveExternalLogin(userLogin));
        }
        
        public Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            var logins = LayerBao.UserBao.GetUserExternalLogins(user.Id);
            return Task.FromResult<IList<UserLoginInfo>>(logins
                .Select(c => new UserLoginInfo(c.LoginProvider, c.ProviderKey)).ToList());
        }

        public Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            var user = LayerBao.UserBao.GetAspUserByExternalLogin(new AspNetUserLogin()
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            });
            return Task.Run(() => ToApplicationUser(user));
        }

        public Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            var claims = LayerBao.UserBao.GetClaims(user.Id);
            return Task.FromResult<IList<Claim>>(claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList());
        }

        public Task AddClaimAsync(ApplicationUser user, Claim claim)
        {
            var userClaim = new AspNetUserClaim()
            {
                UserId = user.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            };
            return Task.Run(() => LayerBao.UserBao.AddUserClaim(userClaim));
        }

        public Task RemoveClaimAsync(ApplicationUser user, Claim claim)
        {
            var userClaim = new AspNetUserClaim()
            {
                UserId = user.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            };
            return Task.Run(() => LayerBao.UserBao.RemoveUserClaim(userClaim));
        }

        public Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            return Task.Run(() => LayerBao.UserBao.AddRoleById(user.Id, roleName));
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            return Task.Run(() => LayerBao.UserBao.RemoveRoleById(user.Id, roleName));
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return Task.FromResult<IList<string>>(LayerBao.UserBao.GetRoles(user.Email));
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            return Task.Run(() => LayerBao.UserBao.HaveRole(user.Email, new List<string> {roleName}));
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            var aspUser = LayerBao.UserBao.GetAspUser(user.Email, user.Id);
            DateTimeOffset offset = aspUser?.LockoutEndDateUtc ?? DateTime.Now;
            return Task.Run(() => offset);
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            var aspUser = LayerBao.UserBao.GetAspUser(user.Email, user.Id);
            aspUser.LockoutEndDateUtc = lockoutEnd.DateTime;
            return Task.Run(() => LayerBao.UserBao.Update(aspUser, user.Id));
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            var aspUser = LayerBao.UserBao.GetAspUser(user.Email, user.Id);
            aspUser.AccessFailedCount++;
            LayerBao.UserBao.Update(aspUser, user.Id);
            return Task.Run(() => aspUser.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            var aspUser = LayerBao.UserBao.GetAspUser(user.Email, user.Id);
            aspUser.AccessFailedCount = 0;
            return Task.Run(() => LayerBao.UserBao.Update(aspUser, user.Id));
        }

        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            var aspUser = LayerBao.UserBao.GetAspUser(user.Email, user.Id);
            return Task.Run(() => aspUser.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            var aspUser = LayerBao.UserBao.GetAspUser(user.Email, user.Id);
            return Task.Run(() => aspUser.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            var aspUser = LayerBao.UserBao.GetAspUser(user.Email, user.Id);
            aspUser.LockoutEnabled = enabled;
            return Task.Run(() => LayerBao.UserBao.Update(aspUser, user.Id));
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            var aspUser = LayerBao.UserBao.GetAspUser(user.Email, user.Id);
            aspUser.TwoFactorEnabled = enabled;
            return Task.Run(() => LayerBao.UserBao.Update(aspUser, user.Id));
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            var aspUser = LayerBao.UserBao.GetAspUser(user.Email, user.Id);
            return Task.Run(() => aspUser.TwoFactorEnabled);
        }

        public Task SetEmailAsync(ApplicationUser user, string email)
        {
            var aspUser = LayerBao.UserBao.GetAspUser(user.Email, user.Id);
            aspUser.Email = email;
            return Task.Run(() => LayerBao.UserBao.Update(aspUser, user.Id));
        }

        public Task<string> GetEmailAsync(ApplicationUser user)
        {
            var aspUser = LayerBao.UserBao.GetAspUser(user.Email, user.Id);
            return Task.Run(() => aspUser.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            var aspUser = LayerBao.UserBao.GetAspUser(user.Email, user.Id);
            return Task.Run(() => aspUser.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            var aspUser = LayerBao.UserBao.GetAspUser(user.Email, user.Id);
            aspUser.EmailConfirmed = confirmed;
            return Task.Run(() => LayerBao.UserBao.Update(aspUser, user.Id));
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var aspUser = LayerBao.UserBao.GetAspUser(email);
            return Task.Run(() => ToApplicationUser(aspUser));
        }
    }
}
