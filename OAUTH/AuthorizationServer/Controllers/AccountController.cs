using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AuthorizationServer.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult External(string provider)
        {
            var authentication = HttpContext.GetOwinContext().Authentication;

            if (!string.IsNullOrEmpty(provider))
            {
                authentication.Challenge(provider);
                return new HttpUnauthorizedResult();
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(bool? isPersistent, string username, string password, string acao)
        {
            var authentication = HttpContext.GetOwinContext().Authentication;

            if (acao == "Autenticar")
            {
                //Aqui seria a validação se o usuário e senha estão corretos
                if (username != password)
                {
                    TempData["Message"] = "Usuário ou senha inválido!";

                    var queryString = new RouteValueDictionary();

                    foreach (var key in Request.QueryString.AllKeys)
                    {
                        queryString.Add(key, Request.QueryString[key]);
                    }

                    return RedirectToAction("Login", queryString);
                }

                authentication.SignIn(
                    new AuthenticationProperties { IsPersistent = isPersistent.HasValue && isPersistent.Value },
                    new ClaimsIdentity(new[] { new Claim(ClaimsIdentity.DefaultNameClaimType, username) }, "Application"));
            }

            return View();
        }

        public ActionResult Logout()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> External()
        {
            var authentication = HttpContext.GetOwinContext().Authentication;
            var auth = await authentication.AuthenticateAsync("External");
            var identity = auth.Identity;

            if (identity != null)
            {
                authentication.SignOut("External");
                authentication.SignIn(
                    new AuthenticationProperties { IsPersistent = true },
                    new ClaimsIdentity(identity.Claims, "Application", identity.NameClaimType, identity.RoleClaimType));
                return Redirect(Request.QueryString["ReturnUrl"]);
            }

            return View();
        }
    }
}