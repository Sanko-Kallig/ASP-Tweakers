using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Tweakers.Models;

namespace Tweakers.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Product(int id = 0)
        {
            Product product = new Product();
            product = product.GetProduct(id);
            product.GetReviews(product);
            return View(product);
        }

        public ActionResult Overview(int id = 0)
        {
            ViewBag.id = id;
            return View(new ViewModel());
        }

        public ActionResult NewProduct(int id = 0)
        {
            Session["CatID"] = id;
            return View();
        }

        [HttpPost]
        public ActionResult NewProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                int id = Convert.ToInt32(Session["CatID"]);
                if (product.AddProduct(product, id))
                {
                    return RedirectToAction("Overview", "Product");
                }
            }
            return View(product);
        }

        public ActionResult UpdateProduct(int id = 0)
        {
            Product product = new Product();
            product = product.GetProduct(id);
            return View(product);
        }

        [HttpPost]
        public ActionResult UpdateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.UpdateProduct(product))
                {
                    return RedirectToAction("Product", new { id = product.ProductID});
                }
            }
            return View(product);
        }

        //public ActionResult Overview(int parentId)
        //{
        //    ViewModel tempModel = new ViewModel();
        //    tempModel.PriceWatch = tempModel.PriceWatch.FindAll((n => n.SubID == parentId));
        //    return View(tempModel);
        //}


    }
}