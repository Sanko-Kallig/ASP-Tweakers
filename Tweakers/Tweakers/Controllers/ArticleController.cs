using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tweakers.Models;

namespace Tweakers.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult Index(string id = "")
        {
            Article currentArticle = new Article();
            currentArticle = currentArticle.GetArticle(id);
            currentArticle.GetReactions(currentArticle);
            Session["CurrentArticle"] = currentArticle;
            return View(currentArticle);
        }

        public ActionResult NewArticle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewArticle(Article article, int catid = 0)
        {
            if (ModelState.IsValid)
            {
                article.AddArticle(article, catid);
            }
            return View(article);
        }

        [HttpPost]
        public ActionResult PlaceReaction(FormCollection collection, Article article)
        {
            article.AddReaction(collection["reactionForm"], article, (Account)Session["User"]);
            return RedirectToAction("Index", "Article", new { id = article.Title +"/"} );
        }

        [HttpPost]
        public ActionResult PlaceSubReaction(FormCollection collection, Article artilce, int id = 0)
        {
            ((Article)Session["CurrentArticle"]).AddSubReaction(collection["reactionSubForm"], id, (Article)Session["CurrentArticle"], (Account)Session["User"]);
            return RedirectToAction("Index", "Article", new { id = ((Article)Session["CurrentArticle"]).Title + "/" });
        }
    }
}