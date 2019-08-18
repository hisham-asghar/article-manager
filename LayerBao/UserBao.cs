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
    }
}
