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
            return View((Account)Session["User"]);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            if (Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
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

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(Account loginAccount)
        {
            if (loginAccount.Email != String.Empty && loginAccount.Password != string.Empty)
            {
                Session["User"] = loginAccount.GetAccount(loginAccount);
                if(Session["User"] != null)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
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