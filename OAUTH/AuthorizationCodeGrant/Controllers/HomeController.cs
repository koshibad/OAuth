using Constantes;
using DotNetOpenAuth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AuthorizationCodeGrant.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            InitializeWebServerClient();
        }

        private WebServerClient _webServerClient;

        public async Task<ActionResult> Index(string acao, string accessToken, string refreshToken)
        {
            ViewBag.AccessToken = Request.Form["AccessToken"] ?? "";
            ViewBag.RefreshToken = Request.Form["RefreshToken"] ?? "";
            ViewBag.Action = "";
            ViewBag.ApiResponse = "";

            if (string.IsNullOrEmpty(acao))
            {
                if (string.IsNullOrEmpty(accessToken))
                {
                    var authorizationState = _webServerClient.ProcessUserAuthorization(Request);
                    if (authorizationState != null)
                    {
                        ViewBag.AccessToken = authorizationState.AccessToken;
                        ViewBag.RefreshToken = authorizationState.RefreshToken;
                        ViewBag.Action = Request.Path;
                    }
                }
            }
            else if (acao == "Authorize")
            {
                var userAuthorization = _webServerClient.PrepareRequestUserAuthorization(new[] { "notas", "administracao" });
                userAuthorization.Send(HttpContext);
                Response.End();
            }
            else if (acao == "Refresh")
            {
                var state = new AuthorizationState
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
                if (_webServerClient.RefreshAuthorization(state))
                {
                    ViewBag.AccessToken = state.AccessToken;
                    ViewBag.RefreshToken = state.RefreshToken;
                }
            }
            else if (acao == "Acessar API protegida")
            {
                var resourceServerUri = new Uri(Paths.ResourceServerBaseAddress);
                var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(accessToken));
                var body = await client.GetStringAsync(new Uri(resourceServerUri, Paths.AlunosPath));
                ViewBag.ApiResponse = body;
            }


            return View();
        }

        private void InitializeWebServerClient()
        {
            var authorizationServerUri = new Uri(Paths.AuthorizationServerBaseAddress);
            var authorizationServer = new AuthorizationServerDescription
            {
                AuthorizationEndpoint = new Uri(authorizationServerUri, Paths.AuthorizePath),
                TokenEndpoint = new Uri(authorizationServerUri, Paths.TokenPath)
            };
            _webServerClient = new WebServerClient(authorizationServer, Clients.Client1.Id, Clients.Client1.Secret);
        }
    }
}