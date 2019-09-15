using LayerDao;
using LayerDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayerBao
{
    public class UserBao
    {
        public static bool IsUserTagExist1(string email)
        {
            return UserDao.IsUserTagExist(email);
        }
        public static void InsertTagId(string email, long id)
        {
            string uid = UserDao.getId(email);
            UserDao.InsertTagId(uid , id );

        }
        public static List<AspNetRole> GetRolesDto(string username)
        {
            return UserDao.GetRolesDto(username);
        }
        public static List<string> GetRoles(string username)
        {
            return UserDao.GetRoles(username);
        }

        public static List<AspNetUser> GetUsersByRoleName(string id)
        {
            return UserDao.GetUsersByRoleName(id);
        }
        public static List<AspNetUser> GetUsersByTagName(string id)
        {
            return UserDao.GetAspNetUsersByTagName(id);
        }

        public static bool UpdateRoles(string userId, string email, List<string> ur)
        {
            var result = true;
            var existingRoles = UserDao.GetRoles(email);
            foreach(var role in existingRoles)
            {
                if (ur.Contains(role))
                {
                    result = ur.Remove(role) && result;
                }else
                {
                    result = UserDao.RemoveRoleById(userId, role) && result;
                }
            }
            foreach(var role in ur)
            {
                result = UserDao.AddRoleById(userId, role) && result;
            }
            return result;
        }

        public static bool Create(AspNetUser aspNetUser)
        {
            return UserDao.Create(aspNetUser);
        }

        public static void Update(AspNetUser aspuser, string id)
        {
            UserDao.UpdateFull(aspuser,id);
        }

        public static bool RemoveUserById(string id)
        {
            return UserDao.RemoveUserById(id);
        }

        public static AspNetUser GetUserById(string id)
        {
            return UserDao.GetAspUserById(id);
        }
        
        public static bool AddRoleById(string id, string role)
        {
            return UserDao.AddRoleById(id, role);
        }

        public static bool RemoveRoleById(string id, string role)
        {
            return RemoveRoleById(id, role);
        }

        public static List<AspNetUserClaim> GetClaims(string userId)
        {
            return UserDao.GetClaims(userId);
        }

        public static bool AddExternalLogin(AspNetUserLogin userLogin)
        {
            return UserDao.AddExternalLogin(userLogin);
        }

        public static List<AspNetUserLogin> GetUserExternalLogins(string userId)
        {
            return UserDao.GetUserExternalLogins(userId);
        }

        public static AspNetUser GetAspUserByExternalLogin(AspNetUserLogin userLogin)
        {
            return UserDao.GetAspUserByExternalLogin(userLogin);
        }
        public static bool RemoveExternalLogin(AspNetUserLogin userLogin)
        {
            return UserDao.RemoveExternalLogin(userLogin);
        }

        public static bool AddUserClaim(AspNetUserClaim claim)
        {
            return UserDao.AddUserClaim(claim);
        }
        public static bool RemoveUserClaim(AspNetUserClaim claim)
        {
            return UserDao.RemoveUserClaim(claim);
        }

        public static bool HaveRole(string email, List<string> list)
        {
            return UserDao.HaveRole(email, list);
        }

        public static AspNetUser GetAspUser(string email, string id = null)
        {
            return UserDao.GetAspUser(email, id);
        }
    }
}
