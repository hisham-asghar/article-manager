using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayerDao;
namespace LayerBao
{
    public static class ArticleManagement
    {
        public static void ReviewRequest(int? id)
        {
            ArticleManager.ReviewRequest(id);
        }

        public static void ArticleReviewed(int? id)
        {
            ArticleManager.ArticleReviewed(id);
        }

        public static void PublishRequest(int? id)
        {
            ArticleManager.PublishRequest(id);
        }

        public static void ArticlePublished(int? id)
        {
            ArticleManager.ArticlePublished(id);
        }
    }
}
