using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constantes
{
    public static class Paths
    {
        /// <summary>
        /// URL do projeto Authorization Server
        /// </summary>
        public const string AuthorizationServerBaseAddress = "http://localhost:41000";

        /// <summary>
        /// URL do projeto Resource Server
        /// </summary>
        public const string ResourceServerBaseAddress = "http://localhost:38385";

        /// <summary>
        /// URL do Callback do projeto ImplicitGrant
        /// </summary>
        public const string ImplicitGrantCallBackPath = "http://localhost:38515/Home/SignIn";

        /// <summary>
        /// URL do projeto Authorization Code Grant
        /// </summary>
        public const string AuthorizeCodeCallBackPath = "http://localhost:38500/";


        public const string AuthorizePath = "/OAuth/Authorize";
        public const string TokenPath = "/OAuth/Token";
        public const string LoginPath = "/Account/Login";
        public const string LogoutPath = "/Account/Logout";

        public const string AlunosPath = "/api/v1/notas/alunos";
    }
}
