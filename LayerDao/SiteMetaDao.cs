using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayerDb;
using LayerDb.Models;

namespace LayerDao
{
    public static class SiteMetaDao
    {
        public static string DatabaseName = "Database";
        public static SiteMeta Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return null;
            var query = $"SELECT * FROM dbo.SiteMeta WHERE Name like '{key}'";
            return QueryExecutor.FirstOrDefault<SiteMeta>(query, $"Get SiteMeta Key = {key}");
        }
        public static string GetDatabase(string d)
        {
            using (var db = new DbModel())
            {
                var database = db.SiteMetas.FirstOrDefault(dt => dt != null &&  dt.Name.Equals(d));
                return database.Value;
            }
        }

        public static bool Set(string key, string value)
        {
            var meta = Get(key);
            if(meta == null)
            {
                var insertQuery = $"INSERT INTO [dbo].[SiteMeta] ([Name] ,[Value]) Values ('{key}','{value}')";
                return QueryExecutor.ExecuteDml(insertQuery);
            }
            else
            {
                var updateQuery = $"Update dbo.SiteMeta SET Value = '{value}' WHERE Name = '{key}'";
                return QueryExecutor.ExecuteDml(updateQuery);
            }
        }

        public  static long GetTagIdOfDatabase()
        {
            string n = GetDatabase(DatabaseName);

            using (var db = new DbModel())
            {
                var tag = db.Tags.Where(t=>t.Name == n).FirstOrDefault();
                return tag.Id;
            }
            
        }
        

    }
}
