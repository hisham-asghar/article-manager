using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayerDb;

namespace LayerDao
{
    public class ArticlesDao
    {
        public static int MyArticlesCount(string userId)
        {
            var query = $"SELECT COUNT(*) AS Result FROM dbo.Articles WHERE CreatedBy = '{userId}'";
            return QueryExecutor.FirstOrDefault<TemplateClass<int>>(query,"My Articles Count = " + userId)?.Result ?? 0;
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
