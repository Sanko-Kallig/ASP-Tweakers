using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tweakers.Models
{
    public class Review
    {
        public int ID { get; set; }
        public Account Reviewer { get; set; }

        public Product Product { get; set; }

        public string Title { get; set; }

        public string Context { get; set; }

        public List<Reaction> Reactions { get; set; }

        public Review(int id, Account reviewer, Product product,string title, string context)
        {
            this.ID = id;
            this.Reviewer = reviewer;
            this.Product = product;
            this.Title = title;
            this.Context = context;
        }

        public Review()
        {
            
        }

        public Review GetReview(int id)
        {
            return DatabaseManager.GetReview(id);
        }

        public void AddReaction(string v, Review review, Account account)
        {
            v = v.Replace("\n", "<br>");
            Reaction reaction = new Reaction(-1, account, DateTime.Now, v);
            DatabaseManager.AddReviewReaction(reaction, review, account);
        }

        public void AddSubReaction(string v, int id, Review review, Account account)
        {
            v = v.Replace("\n", "<br>");
            Reaction reaction = new Reaction(-1, account, DateTime.Now, v);
            DatabaseManager.AddSubReviewReaction(reaction, id, review, account);
        }

        public bool AddReview(Review review)
        {
            return DatabaseManager.AddReview(review);
        }

        public Review GetReview(string title)
        {
            return DatabaseManager.GetReview(title);
        }

        public void GetReactions(Review review)
        {
            Reactions = DatabaseManager.GetReactions(review);
        }

        public bool UpdateReview(Review review)
        {
            return DatabaseManager.UpdateReview(review);
        }
    }
}