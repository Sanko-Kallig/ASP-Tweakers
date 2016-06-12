using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tweakers.Models;

namespace Tweakers.Controllers
{
    public class ReviewController : Controller
    {
        // GET: Review
        public ActionResult Index(string id = "")
        {
            Review currentReview = new Review();
            currentReview = currentReview.GetReview(id);
            currentReview.GetReactions(currentReview);
            Session["CurrentReview"] = currentReview;
            return View(currentReview);
        }

        public ActionResult NewReview(int id = 0)
        {
            Product product = new Product();
            product = product.GetProduct(id);
            Review review = new Review();
            review.Product = product;
            return View(review);
        }

        [HttpPost]
        public ActionResult NewReview(Review review)
        {
            if (ModelState.IsValid)
            {
                review.Reviewer = (Account) Session["User"];
                if (review.AddReview(review))
                {
                    return RedirectToAction("Index", new {id = review.Product.Name});
                }
            }
            return View(review);
        }

        public ActionResult UpdateReview(string id = "")
        {
            Review currentReview = new Review();
            currentReview = currentReview.GetReview(id);
            return View(currentReview);
        }

        public ActionResult UpdateReview(Review review)
        {
            if (ModelState.IsValid)
            {
                if (review.UpdateReview(review))
                {
                    return RedirectToAction("Index", new { id = review.Product.Name });
                }
            }
            return View(review);
        }

        [HttpPost]
        public ActionResult PlaceReaction(FormCollection collection, Review review)
        {
            review.AddReaction(collection["reactionForm"], review, (Account)Session["User"]);
            return RedirectToAction("Index", "Review", new { id = review.Title + "/" });
        }

        [HttpPost]
        public ActionResult PlaceSubReaction(FormCollection collection, Review review, int id = 0)
        {
            review.AddSubReaction(collection["reactionSubForm"], id, review, (Account)Session["User"]);
            return RedirectToAction("Index", "Review", new { id = review.Title + "/" });
        }
    }
}