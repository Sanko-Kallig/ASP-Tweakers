using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tweakers.Models
{
    public class ViewModel
    {
        public List<PriceWatchCategory> PriceWatch = new List<PriceWatchCategory>();

        public List<Article> Articles = new List<Article>();

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