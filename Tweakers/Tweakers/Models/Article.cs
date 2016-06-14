using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tweakers.Models
{
    public class Article
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }

        public Account Author { get; set; }

        public DateTime PublicationDate { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Context { get; set; }

        public List<Reaction> Reactions { get; set; }


        public Article()
        {
            
        }

        public Article(int id, string title, Account author, DateTime publicationDate, string context)
        {
            this.ID = id;
            this.Title = title;
            this.Author = author;
            this.PublicationDate = publicationDate;
            this.Context = context;
        }

        public bool AddArticle(Article article, int catid)
        {
            return DatabaseManager.AddArticle(article, catid);
        }

        public bool UpdateArticle(Article article)
        {
            return DatabaseManager.UpdateArticle(article);
        }

        public void AddReaction(string v, Article article, Account account)
        {
            v = v.Replace("\n", "<br>");
            Reaction reaction = new Reaction(-1, account, DateTime.Now, v);
            DatabaseManager.AddArticleReaction(reaction, article, account);
        }

        public void AddSubReaction(string v, int id, Article article, Account account)
        {
            v = v.Replace("\n", "<br>");
            Reaction reaction = new Reaction(-1, account, DateTime.Now, v);
            DatabaseManager.AddSubArticleReaction(reaction,id , article, account);
        }

        public List<Article> GetArticles()
        {
            return DatabaseManager.GetArticles();
        }

        public Article GetArticle(string id)
        {
            return DatabaseManager.GetArticle(id);

        }

        public void GetReactions(Article article)
        {
            this.Reactions = DatabaseManager.GetReactions(article);
            this.Reactions.Sort((x, y) => y.PostTime.CompareTo(x.PostTime));
        }
    }
}