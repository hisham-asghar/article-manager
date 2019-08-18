﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LayerDao;
using LayerDb;
using LayerDb.Models;
using Generics;

namespace LayerDao
{
    public static class UserDao
    {

        public static List<AspNetUser> GetAspNetUsers()
        {
            var query = "Select * from dbo.AspNetUsers";
            return QueryExecutor.List<AspNetUser>(query, "Get Users List");
        }

        public static List<AspNetUser> GetUsersByRoleId(string roleId)
        {
            if (string.IsNullOrWhiteSpace(roleId)) return new List<AspNetUser>();
            var query = $"Select * from dbo.AspNetUsers WHERE Id IN (SELECT [UserId] FROM [dbo].[AspNetUserRoles] WHERE RoleId = '{roleId}')";
            return QueryExecutor.List<AspNetUser>(query, "Get Users List By Role Id = " + roleId);
        }

        public static List<AspNetUser> GetUsersByRoleName(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName)) return new List<AspNetUser>();
            var query = $"Select * from dbo.AspNetUsers WHERE Id IN (SELECT [UserId] FROM [dbo].[AspNetUserRoles] WHERE RoleId = (SELECT TOP 1 Id FROM [dbo].[AspNetRoles]) WHERE Name like {roleName})";
            return QueryExecutor.List<AspNetUser>(query, "Get Users List By Role Name = " + roleName);
        }

        public static AspNetUser GetAspUser(string email, string id = null)
        {
            var query = UserQueries.GetAspUserQuery(id, email);
            return QueryExecutor.FirstOrDefault<AspNetUser>(query, "Get Asp User By Email");
        }


        public static AspNetUser GetAspUserById(string id)
        {
            var query = UserQueries.GetAspUserQuery(id, null);
            return QueryExecutor.FirstOrDefault<AspNetUser>(query, "Get Asp User By Email");
        }


        //public static bool HaveRole(string username, List<string> roles)
        //{
        //    if (string.IsNullOrWhiteSpace(username) || roles == null || roles.Count == 0) return false;
        //    var listStr = roles.Count == 1
        //        ? $"'{roles.Select(s => s?.ToLower()).FirstOrDefault()}'"
        //        : $"{roles.Select(s => "'" + s?.ToLower() + "'").Aggregate((c, n) => c + "," + n)}";

        //    var query =
        //        "SELECT TOP 1 [PersonName],[Email],[UserName],[Role] " +
        //        "FROM [dbo].[UserRolesView] " +
        //        $"WHERE Email = '{username}' and LOWER(Role) IN ({listStr});";
        //    return QueryExecutor.FirstOrDefault<UserRolesView>(query) != null;
        //}


        public static List<AspNetRole> GetRolesDto(string username)
        {

            if (string.IsNullOrWhiteSpace(username))
            {
                var allRoles = "SELECT * FROM [dbo].[AspNetRoles]";
                return QueryExecutor.List<AspNetRole>(allRoles, "Get All Roles Dto");
            }
            var query =
                "SELECT * FROM dbo.AspNetRoles INNER JOIN dbo.AspNetUserRoles ON dbo.AspNetRoles.Id = dbo.AspNetUserRoles.RoleId " +
                $"WHERE dbo.AspNetUserRoles.UserId = (SELECT TOP 1 Id FROM dbo.AspNetUsers WHERE Email = '{username}')";
            var list = QueryExecutor.List<AspNetRole>(query, "Get Roles Dto of User = " + username) ?? new List<AspNetRole>();
            return list;
        }
        public static List<string> GetRoles(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                var allRoles = "SELECT [Name] AS Result FROM [dbo].[AspNetRoles]";
                return QueryExecutor.List<TemplateClass<string>>(allRoles, "Get All Roles")?.Select(r => r.Result)
                    .ToList();
            }
            var query =
                "SELECT Name AS Result FROM dbo.AspNetRoles INNER JOIN dbo.AspNetUserRoles ON dbo.AspNetRoles.Id = dbo.AspNetUserRoles.RoleId " +
                $"WHERE dbo.AspNetUserRoles.UserId = (SELECT TOP 1 Id FROM dbo.AspNetUsers WHERE Email = '{username}')";
            var list = QueryExecutor.List<TemplateClass<string>>(query, "Get Roles of User = " + username) ?? new List<TemplateClass<string>>();
            return list.Select(u => u.Result).ToList();
        }

        //public static bool Update(AspNetUser user, string id, bool force = false)
        //{
        //    using (var db = new DbModel())
        //    {
        //        var userInfo = db.AspNetUsers.FirstOrDefault(u => u.Id == id);
        //        if (userInfo == null) return false;
        //        try
        //        {
        //            userInfo.PhoneNumber = user.PhoneNumber;
        //            userInfo.PersonName = user.PersonName;
        //            userInfo.UserName = user.PersonName;
        //            userInfo.HomeTown = user.HomeTown;
        //            if(user.PasswordHash != null)
        //            {
        //                userInfo.PasswordHash = user.PasswordHash;
        //            }
        //            userInfo.SecondaryPhoneNumber = user.SecondaryPhoneNumber;
        //            userInfo.SecondaryEmailAddress = user.SecondaryEmailAddress;
        //            if (force)
        //            {
        //                userInfo.Email = user.Email;
        //            }

        //            db.SaveChanges();
        //            return true;
        //        }
        //        catch (Exception e)
        //        {
        //            return false;
        //        }
        //    }
        //}

        public static bool UpdateFull(AspNetUser user, string id, bool force = false)
        {
            var updateQuery =
                "UPDATE [dbo].[AspNetUsers] SET " +
                $"[Name] = {user.Name.GetStirngForDb()}" +
                //$",[HomeTown] = {user.HomeTown.GetStirngForDb()}" +
                //$",[BirthDate] = {(user.BirthDate?.ToString()).GetStirngForDb()}" +
                $",[Email] = {user.Email.GetStirngForDb()}" +
                //$",[EmailConfirmed] = {(user.EmailConfirmed ? 1 : 0)}," +
                //$"[PasswordHash] = {user.PasswordHash.GetStirngForDb()}" +
                //$",[SecurityStamp] = {user.SecurityStamp.GetStirngForDb()}" +
                $",[PhoneNumber] = {user.PhoneNumber.GetStirngForDb()}," +
                $"[PhoneNumberConfirmed] = {(user.PhoneNumberConfirmed ? 1 : 0)}," +
                $"[TwoFactorEnabled] = {(user.TwoFactorEnabled ? 1 : 0)}," +
                $"[LockoutEndDateUtc] = {(user.LockoutEndDateUtc?.ToString()).GetStirngForDb()}," +
                $"[LockoutEnabled] = {(user.LockoutEnabled ? 1 : 0)}," +
                $"[AccessFailedCount] = {user.AccessFailedCount}," +
                $"[UserName] = {user.UserName.GetStirngForDb()}" +
                //$",[Address] = {user.Address.GetStirngForDb()}," +
                //$"[Image] = {user.Image.GetStirngForDb()}," +
                //$"[SecondaryEmailAddress] = {user.SecondaryEmailAddress.GetStirngForDb()}," +
                //$"[SecondaryPhoneNumber] = {user.SecondaryPhoneNumber.GetStirngForDb()}" +
                $"WHERE Id = {user.Id.GetStirngForDb()}";

            var query = string.Format(UserQueries.AspUserExistsQuery(user.Id, null), updateQuery);
            return QueryExecutor.ExecuteDml(query, $"Update Asp User Id = {user.Id}, Email = {user.Email}");
        }

        //public static bool Create(AspNetUser user)
        //{
        //    var insertQuery =
        //        "INSERT INTO [dbo].[AspNetUsers] ([Id],[EmailConfirmed],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEnabled],[AccessFailedCount],[UserName]) VALUES ";
        //    insertQuery += $"('{user.Id}',{(user.EmailConfirmed ? 1 : 0)},{(user.PhoneNumberConfirmed ? 1 : 0)},{(user.TwoFactorEnabled ? 1 : 0)},{(user.LockoutEnabled ? 1 : 0)},{user.AccessFailedCount},'{user.UserName}')";
        //    var query = string.Format(UserQueries.AspUserExistsQuery(user.Id, null), insertQuery);
        //    return QueryExecutor.ExecuteDml(query, $"Create Asp User Id = {user.Id}, Email = {user.Email}") &&
        //           UpdateFull(user, user.Id, true);
        //}

        public static string GetStirngForDb(this string str) => str == null ? "NULL" : $"'{str}'";

        //public static bool UpdateRoles(string id, List<string> roles)
        //{
        //    using (var db = new DbModel())
        //    {
        //        try
        //        {
        //            roles = roles.Select(r => r.ToLower()).ToList();
        //            var rolesId = db.AspNetRoles.Where(u => roles.Contains(u.Name.ToLower())).ToList();

        //            var user = db.AspNetUsers.FirstOrDefault(u => u.Id == id);
        //            if (user == null) return false;

        //            var existingRoles = db.AspNetRoles;
        //            foreach (var role in existingRoles)
        //            {
        //                var ou = db.AspNetUsers.FirstOrDefault(u => u.Id == id);
        //                var or = db.AspNetRoles.FirstOrDefault(r => r.Id == role.Id);
        //                if(ou != null && or != null)
        //                    ou.AspNetRoles.Remove(or);
        //            }
        //            try
        //            {
        //                db.SaveChanges();
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine(e);
        //                new ErrorOccurModel()
        //                {
        //                    Exception = e.ToString(),
        //                    ErrorSubject = user.PersonName + " - " + user.Email + " -- <br />" + (e.TargetSite != null ? e.TargetSite.ToString() : "") + e.GetType().ToString()
        //                }.Notify();
        //                return false;
        //                //throw;
        //            }

        //            foreach (var role in rolesId)
        //            {
        //                var nu = user;
        //                var nr = role;
        //                //var nu = new AspNetUser() {Id = id};
        //                ////db.AspNetUsers.Add(nu);
        //                //db.AspNetUsers.Attach(nu);

        //                //var nr = new AspNetRole() {Id = role.Id};
        //                ////db.AspNetRoles.Add(nr);
        //                //db.AspNetRoles.Attach(nr);

        //                nu.AspNetRoles.Add(nr);
        //            }


        //            try
        //            {
        //                db.SaveChanges();
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine(e);
        //                new ErrorOccurModel()
        //                {
        //                    Exception = e.ToString(),
        //                    ErrorSubject = user.PersonName + " - " + user.Email + " -- <br />" + (e.TargetSite != null ? e.TargetSite.ToString() : "") + e.GetType().ToString()
        //                }.Notify();
        //                return false;
        //                //throw;
        //            }
        //            return true;
        //        }
        //        catch (Exception e)
        //        {
        //            new ErrorOccurModel()
        //            {
        //                Exception = e.ToString(),
        //                ErrorSubject = id + " -- <br />" + (e.TargetSite != null ? e.TargetSite.ToString() : "") + e.GetType().ToString()
        //            }.Notify();
        //            return false;
        //        }
        //    }
        //}


        public static bool RemoveUserById(string id)
        {
            var query = $"DELETE FROM dbo.AspNetUsers WHERE Id = '{id}'";
            return QueryExecutor.ExecuteDml(query, "Delete User By Id = " + id);
        }

        public static bool AddRoleById(string id, string role)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(role)) return false;
            var insertQuery = "INSERT INTO [dbo].[AspNetUserRoles] ([UserId],[RoleId]) VALUES" +
                              $"('{id}',(SELECT Id From dbo.AspNetRoles WHERE LOWER(Name) = '{role.Guid()}'))";
            var query = string.Format(UserQueries.AspUserExistsQuery(id, null),
                string.Format(UserQueries.RoleExistsQuery(null, role), insertQuery));
            return QueryExecutor.ExecuteDml(query, "Add Role By Id");
            
        }

        public static bool RemoveRoleById(string id, string role)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(role)) return false;
            var insertQuery = "DELETE FROM [dbo].[AspNetUserRoles] " +
                              $"WHERE UserId = '{id}' AND " +
                              $"RoleId = (SELECT Id From dbo.AspNetRoles WHERE LOWER(Name) = '{role.Guid()}')";
            var query = string.Format(UserQueries.AspUserExistsQuery(id, null),
                string.Format(UserQueries.RoleExistsQuery(null, role), insertQuery));
            return QueryExecutor.ExecuteDml(query, "Remove Role By Id");
            
        }

        public static List<AspNetUserClaim> GetClaims(string userId)
        {
            if(string.IsNullOrWhiteSpace(userId)) return new List<AspNetUserClaim>();
            var query = $"SELECT * FROM [dbo].[AspNetUserClaims] WHERE UserId = '{userId}'";
            return QueryExecutor.List<AspNetUserClaim>(query, "Get User Claims by Id, User Id = " + userId) ??
                   new List<AspNetUserClaim>();

        }

        public static bool AddExternalLogin(AspNetUserLogin userLogin)
        {
            var insertQuery = "INSERT INTO [dbo].[AspNetUserLogins]([LoginProvider],[ProviderKey],[UserId]) VALUES" +
                              $"('{userLogin.LoginProvider}','{userLogin.ProviderKey}','{userLogin.UserId}')";
            var query = string.Format(UserQueries.AspUserExistsQuery(userLogin.UserId, null),
                insertQuery);
            return QueryExecutor.ExecuteDml(query, $"Add User External Login, Id = {userLogin.UserId}, Provider = {userLogin.LoginProvider}");
        }

        public static List<AspNetUserLogin> GetUserExternalLogins(string userId)
        {
            var query = "SELECT * FROM [dbo].[AspNetUserLogins]  WHERE " +
                              $"[UserId] = '{userId}'";
            return QueryExecutor.List<AspNetUserLogin>(query, $"Get User External Login, Id = {userId}");
        }

        public static AspNetUser GetAspUserByExternalLogin(AspNetUserLogin userLogin)
        {
            var query = "SELECT * FROM dbo.AspNetUsers WHERE Id IN" +
                        "(SELECT UserId FROM [dbo].[AspNetUserLogins]  WHERE " +
                        $"[LoginProvider] = '{userLogin.LoginProvider}' AND " +
                        $"[ProviderKey] = '{userLogin.ProviderKey}')";
            return QueryExecutor.FirstOrDefault<AspNetUser>(query,
                $"Get User External Login, Provider = {userLogin.LoginProvider}, Key = {userLogin.ProviderKey}");
        }
        public static bool RemoveExternalLogin(AspNetUserLogin userLogin)
        {
            var insertQuery = "DELETE FROM [dbo].[AspNetUserLogins] WHERE " +
                              $"[LoginProvider] = '{userLogin.LoginProvider}' AND " +
                              $"[ProviderKey] = '{userLogin.ProviderKey}' AND " +
                              $"[UserId] = '{userLogin.UserId}'";
            var query = string.Format(UserQueries.AspUserExistsQuery(userLogin.UserId, null),
                insertQuery);
            return QueryExecutor.ExecuteDml(query, $"Remove User External Login, Id = {userLogin.UserId}, Provider = {userLogin.LoginProvider}");
        }

        public static bool AddUserClaim(AspNetUserClaim claim)
        {
            var insertQuery = "INSERT INTO [dbo].[AspNetUserClaims] ([UserId],[ClaimType],[ClaimValue]) VALUES" +
                              $"('{claim.UserId}','{claim.ClaimType}','{claim.ClaimValue}')";
            var query = string.Format(UserQueries.AspUserExistsQuery(claim.UserId, null),
                insertQuery);
            return QueryExecutor.ExecuteDml(query, $"Add User Claim, Id = {claim.UserId}");
        }
        public static bool RemoveUserClaim(AspNetUserClaim claim)
        {
            var insertQuery = "DELETE [dbo].[AspNetUserClaims] WHERE " +
                              $"[ClaimType] = '{claim.ClaimType}' AND " +
                              $"[ClaimValue] = '{claim.ClaimValue}' AND " +
                              $"[UserId] = '{claim.UserId}'";
            var query = string.Format(UserQueries.AspUserExistsQuery(claim.UserId, null),
                insertQuery);
            return QueryExecutor.ExecuteDml(query, $"Remove User Claim, Id = {claim.UserId}");
        }
    }
    public class UserQueries
    {
        public static string AspUserExistsQuery(string id, string email) =>
            $" BEGIN IF EXISTS (SELECT * FROM [dbo].[AspNetUsers] (nolock)  " +
            $"WHERE Id = '{id}' OR Email = '{email}') " +
            "BEGIN {0} END END ";
        public static string RoleExistsQuery(string id, string roleName) =>
            $" BEGIN IF EXISTS (SELECT * FROM [dbo].[AspNetRoles] (nolock)  " +
            $"WHERE LOWER(Name) = '{roleName}' OR Id = '{id}') " +
            "BEGIN {0} END END ";

        public static string GetAspUserQuery(string id, string email) =>
            $" SELECT * FROM [dbo].[AspNetUsers] (nolock)  " +
            $"WHERE Id = '{id}' OR Email = '{email}';";
    }
}
