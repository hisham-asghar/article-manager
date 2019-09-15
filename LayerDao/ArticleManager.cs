using LayerDb;
using LayerDb.Models;
namespace LayerDao
{
    public static class ArticleManager
    {        
        public static bool ReviewRequest(int? id)
        {
            //var article = LayerDao.ArticlesDao.
            if (id == null) return false;

            var query = "Update dbo.Articles SET IsReadyToReview = 1 WHERE Id = " + id;
            return QueryExecutor.ExecuteDml(query);
            //using (var db = new DbModel())
            //{
            //    var article = db.Articles.Where(s => s.Id == id).FirstOrDefault();
            //    article.IsReadyToReview = true;
            //    db.SaveChanges();
            //}
        }

        public static bool PublishRequest(int? id)
        {
            if (id == null) return false;

            var query = "Update dbo.Articles SET IsReadyToPublish = 1 WHERE Id = " + id;
            return QueryExecutor.ExecuteDml(query);
            //using (var db = new DbModel())
            //{
            //    var article = db.Articles.Find(id);
            //    article.IsReadyToPublish = true;
            //    db.SaveChanges();
            //}
        }

        public static void ArticlePublished(int? id)
        {
            
        }

        public static void ArticleReviewed(int? id)
        {
            
        }
    }
}
