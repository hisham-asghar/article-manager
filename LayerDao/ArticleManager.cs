using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayerDb.Models;
namespace LayerDao
{
    public static class ArticleManager
    {
        
        public static void ReviewRequest(int? id)
        {
            using (var db = new DbModel())
            {
                var article = db.Articles.Where(s => s.Id == id).FirstOrDefault();
                article.IsReadyToReview = true;
                db.SaveChanges();
            }
        }

        public static void PublishRequest(int? id)
        {
            using (var db = new DbModel())
            {
                var article = db.Articles.Find(id);
                article.IsReadyToPublish = true;
                db.SaveChanges();
            }
        }

        public static void ArticlePublished(int? id)
        {
            
        }

        public static void ArticleReviewed(int? id)
        {
            
        }
    }
}
