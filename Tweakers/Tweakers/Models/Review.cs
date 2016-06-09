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

    }
}