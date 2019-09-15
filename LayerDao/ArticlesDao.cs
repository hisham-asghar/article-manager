using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayerDb;
using LayerDb.Models;

namespace LayerDao
{
    public class ArticlesDao
    {
        public static int MyArticlesCount(string userId)
        {
            var query = $"SELECT COUNT(*) AS Result FROM dbo.Articles WHERE CreatedBy = '{userId}'";
            return QueryExecutor.FirstOrDefault<TemplateClass<int>>(query,"My Articles Count = " + userId)?.Result ?? 0;
        }

        public static List<LayerDb.Models.ArticleManagerView> GetArticleManagerView()
        {
            var query = "SELECT * FROM  dbo.ArticleManagerView WHERE IsDeleted = 0";
            return QueryExecutor.List<ArticleManagerView>(query, "Article Manager View");
        }

        public static List<LayerDb.Models.ArticleManagerView> GetArticleManagerViewByUserId( string userId, long tagId)
        {
            var query = $"SELECT * FROM  dbo.ArticleManagerView " +
                $"WHERE ArticleManagerView.CreatedBy = '{userId}' and dbo.ArticleManagerView.TagId = {tagId}";
            return QueryExecutor.List<ArticleManagerView>(query, "Article Manager View By Tag");
        }
        



        public static bool MarkForReview(long id)
        {
            var query = $"UPDATE [dbo].[Articles] SET [IsReadyToReview] = 1 WHERE Id = {id};";
            return QueryExecutor.ExecuteDml(query, "Mark Article Ready for Review = " + id);
        }
        public static bool MarkForPublish(long id)
        {
            var query = $"UPDATE [dbo].[Articles] SET [IsReadyToPublish] = 1 WHERE Id = {id};";
            return QueryExecutor.ExecuteDml(query, "Mark Article Ready for Publish = " + id);
        }
    }
}
