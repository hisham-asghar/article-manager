using LayerDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayerBao
{
    public class ArticlesBao
    {
        public static List<LayerDb.Models.ArticleManagerView> GetArticleManagerView(string databaseName = null)
        {
            var articles = ArticlesDao.GetArticleManagerView();
            if (string.IsNullOrWhiteSpace(databaseName) == false)
                articles = articles.Where(a => a.TagName?.ToLower().Trim() == databaseName?.Trim().ToLower()).ToList();
            return articles;
        }

        public static List<LayerDb.Models.ArticleManagerView> GetArticleManagerViewByUserId(string userId, long tagId)
        {
            return ArticlesDao.GetArticleManagerViewByUserId(userId, tagId);
        }

        public static int MyArticlesCount(string userId)
        {
            return LayerDao.ArticlesDao.MyArticlesCount(userId);
        }
        public static int GetArticles(string userId)
        {
            return LayerDao.ArticlesDao.MyArticlesCount(userId);
        }
       

        public static bool MarkForReview(long id)
        {
            return ArticlesDao.MarkForReview(id);
        }
        public static bool MarkForPublish(long id)
        {
            return ArticlesDao.MarkForPublish(id);
        }
    }
}
