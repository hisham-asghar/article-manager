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

        public static bool UpdateRoles(string id, List<string> ur)
        {
            return true;
        }

        public static void Update(AspNetUser aspuser, string id)
        {
            throw new NotImplementedException();
        }

        public static bool RemoveUserById(string id)
        {
            return UserDao.RemoveUserById(id);
        }
    }
}
