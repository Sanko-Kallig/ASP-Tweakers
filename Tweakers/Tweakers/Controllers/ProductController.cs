﻿using System;
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

        public ActionResult Product(int productid)
        {
            return Content("id=" + productid);
        }

        public ActionResult Overview(int id = 0)
        {
            ViewBag.id = id;
            return View(new ViewModel());
        }

        //public ActionResult Overview(int parentId)
        //{
        //    ViewModel tempModel = new ViewModel();
        //    tempModel.PriceWatch = tempModel.PriceWatch.FindAll((n => n.SubID == parentId));
        //    return View(tempModel);
        //}


    }
}