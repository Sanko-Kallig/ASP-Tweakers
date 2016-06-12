
namespace Tweakers.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Tweakers.Models;


    /// <summary>
    /// Controller class for communication between the Model Account and any views that use Account.
    /// </summary>
    public class AccountController : Controller
    {

        /// <summary>
        /// References a profile view, and populates it with information.
        /// </summary>
        /// <returns>An account model stored in the session to populate the view.</returns>
        public ActionResult Profile()
        {
            return View((Account)Session["User"]);
        }

        /// <summary>
        /// A login view build on the account model
        /// </summary>
        /// <returns>An empty account model.</returns>
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// A logout view used only to clear the session.
        /// </summary>
        /// <returns>Redirects to the homepage if logout succesful else</returns>
        public ActionResult Logout()
        {
            Session["User"] = null;
            if (Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();

        }


        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult UpdateProfile()
        {
            return View((Account)Session["User"]);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Account loginAccount)
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
        public ActionResult Register(FormCollection formCollection, Account registeraccount)
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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(Account updateAccount)
        {
            if (ModelState.IsValid)
            {
                if (updateAccount.UpdateAccount(updateAccount))
                {
                    Session["User"] = updateAccount;
                    return RedirectToAction("Profile", "Account");
                }
            }
            return View(updateAccount);
        }
    }
}