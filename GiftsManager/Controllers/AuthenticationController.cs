using GiftsManager.Models;
using GiftsManager.Models.Dal;
using GiftsManager.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GiftsManager.Controllers
{
    public class AuthenticationController : BaseController
    {
        private IDalUser dalUser;

        public AuthenticationController() : this(new DalUser())
        {

        }

        private AuthenticationController(IDalUser dalIoc)
        {
            dalUser = dalIoc;
        }

        public ActionResult SignIn()
        {
            SignInViewModel viewModel = new SignInViewModel { Authenticated = HttpContext.User.Identity.IsAuthenticated };

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                viewModel.User = dalUser.GetUserByEmail(HttpContext.User.Identity.Name);
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SignIn(SignInViewModel viewModel, string returnUrl)
        {
            if (viewModel.User != null)
            {
                User user = dalUser.Authenticate(viewModel.User.Email, viewModel.User.Password);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);

                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return Redirect("/");
                }

                ModelState.AddModelError("User.FirstName", "Email/Password did not match");
            }

            return View(viewModel);
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(User user)
        {
            // TODO: Check if the user exists
            if (ModelState.IsValid)
            {
                dalUser.AddUser(user);
                FormsAuthentication.SetAuthCookie(user.Email, false);
                return Redirect("/");
            }

            return View(user);
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}