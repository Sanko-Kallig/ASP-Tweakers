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

        [HttpPost]
        public ActionResult PlaceReaction(FormCollection collection)
        {
            ((Review)Session["CurrentReview"]).AddReaction(collection["reactionForm"], (Review)Session["CurrentReview"], (Account)Session["User"]);
            return RedirectToAction("Index", "Review", new { id = ((Review)Session["CurrentReview"]).Title + "/" });
        }
    }
}