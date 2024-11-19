using System;
using System.Diagnostics;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

namespace BlingApiDailyConsult.Infrastructure
{
    // Classe para tratar da obtenção do Authorization Code
    public class OAuthHelperGetAuthCode
    {
        public static void RedirectToAuthUrl()
        {
            string clientId = "95816cb9fddf7d246bf47504382c7de6267abd33";  // Fornecido na Aplicacao no Bling
            string state = "1213141516171819";  // Pode ser um valor aleatório
            string redirectUri = "http://localhost:3000/callback";  // URL de redirecionamento definida ba Aplicacao no Bling

            string authorizationUrl = $"https://bling.com.br/Api/v3/oauth/authorize?response_type=code&client_id={clientId}&state={state}&redirect_uri={redirectUri}";

            // Redireciona o usuário para o navegador
            Process.Start(new ProcessStartInfo(authorizationUrl) { UseShellExecute = true });
        }
    }
}

