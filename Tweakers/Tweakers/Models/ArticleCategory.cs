using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tweakers.Models
{
    public class ArticleCategory
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public List<Article> CatArticles { get { return DatabaseManager.GetCatArticles(ID); } }
        
        public ArticleCategory(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}