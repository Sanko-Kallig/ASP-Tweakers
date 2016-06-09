using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tweakers.Models;

namespace Tweakers.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            
            return View(new ViewModel());
        }

        public ActionResult About()
        {
            return View();
        }
    }
}