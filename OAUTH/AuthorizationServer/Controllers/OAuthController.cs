using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AuthorizationServer.Controllers
{
    public class OAuthController : Controller
    {
        public async Task<ActionResult> Authorize()
        {
            if (Response.StatusCode != 200)
            {
                return View("AuthorizeError");
            }

            var authentication = HttpContext.GetOwinContext().Authentication;
            var ticket = await authentication.AuthenticateAsync("Application");
            var identity = ticket != null ? ticket.Identity : null;
            if (identity == null)
            {
                authentication.Challenge("Application");
                return new HttpUnauthorizedResult();
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Authorize(string acao, string scope)
        {
            if (Response.StatusCode != 200)
            {
                return View("AuthorizeError");
            }

            var authentication = HttpContext.GetOwinContext().Authentication;
            var ticket = await authentication.AuthenticateAsync("Application");
            var identity = ticket != null ? ticket.Identity : null;

            if (identity == null)
            {
                return View("AuthorizeError");
            }

            var scopes = scope.Split(' ');

            if (acao == "Permitir")
            {
                identity = new ClaimsIdentity(identity.Claims, OAuthDefaults.AuthenticationType, identity.NameClaimType, identity.RoleClaimType);
                foreach (var item in scopes)
                {
                    identity.AddClaim(new Claim("urn:oauth:scope", item));
                }
                authentication.SignIn(identity);
            }
            else if (acao == "Logar com outro usuário")
            {
                authentication.SignOut("Application");
                authentication.Challenge("Application");
                return new HttpUnauthorizedResult();
            }

            return View();
        }
    }
}