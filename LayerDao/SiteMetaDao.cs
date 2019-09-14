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
        public static string GetDatabase(string d)
        {
            using (var db = new DbModel())
            {

                
               
                var database = db.SiteMetas.FirstOrDefault(dt => dt != null &&  dt.Name.Equals(d));
                return database.Value;
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
