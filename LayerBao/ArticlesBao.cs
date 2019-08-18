using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayerBao
{
    public class ArticlesBao
    {
        public static int MyArticlesCount(string userId)
        {
            return LayerDao.ArticlesDao.MyArticlesCount(userId);
        }
    }
}
