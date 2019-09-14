using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayerDao;
namespace LayerBao
{
    public static class SiteMetaBao
    {
        public static  long GetCurrentDatabase()
        {
            return SiteMetaDao.GetTagIdOfDatabase();
        }
        
    }
}
