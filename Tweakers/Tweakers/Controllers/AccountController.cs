using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tweakers.Models;

namespace Tweakers.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Profile()
        {
            var account = new Account() {UserName = "Sander", Type = "Admin"};
            return View(account);
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View(new ViewModel());
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult UpdateProfile()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(FormCollection formCollection, Account registeraccount)
        {
            if (ModelState.IsValid)
            {
                Account account = new Account();
                account.UserName = formCollection["UserName"];
                account.FirstName = formCollection["FirstName"];
                account.Email = formCollection["Email"];
                account.Password = formCollection["Password"];

                if (account.CreateAccount(account))
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(registeraccount);

        }
    }
}