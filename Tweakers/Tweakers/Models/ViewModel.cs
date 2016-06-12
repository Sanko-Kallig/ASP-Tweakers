using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tweakers.Models
{
    public class ViewModel
    {
        public List<PriceWatchCategory> PriceWatch { get; set; }

        public List<Article> Articles { get; set; }

        public List<ArticleCategory> ArticleCategories { get; set; }

        public ViewModel()
        {
            PriceWatch = GetPriceWatch();
            Articles = GetArticles();

        }

        private List<Product> GetProducts()
        {
            return DatabaseManager.GetProducts();
        }

        public List<PriceWatchCategory> GetPriceWatch()
        {
            return DatabaseManager.GetPriceWatch();
        }

        public List<Article> GetArticles()
        {
            Article article = new Article();
            return article.GetArticles();
        }

    }
}