using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tweakers.Models
{
    public class Article
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public Account Author { get; set; }

        public DateTime PublicationDat { get; set; }

        public string Context { get; set; }

        public List<Reaction> Reactions { get; set; }


        public Article()
        {
            
        }

        public Article(int id, string title, Account author, DateTime publicationDat, string context)
        {
            this.ID = id;
            this.Title = title;
            this.Author = author;
            this.PublicationDat = publicationDat;
            this.Context = context;
        }

        public List<Article> GetArticles()
        {
            return DatabaseManager.GetArticles();
        }
    }
}