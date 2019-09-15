using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayerDao;
using LayerDb.Models;

namespace LayerBao
{
    public static class SiteMetaBao
    {
        public static SiteMeta GetCurrentDatabase()
        {
            var database = "DefaultDatabase";
            return SiteMetaDao.Get(database);
        }
        public static string GetCurrentDatabaseName()
        {
            var database = "DefaultDatabase";
            return SiteMetaDao.Get(database)?.Value;
        }
        public static bool SetCurrentDatabaseName(string name)
        {
            var database = "DefaultDatabase";
            return SiteMetaDao.Set(database,name);
        }

    }
}
