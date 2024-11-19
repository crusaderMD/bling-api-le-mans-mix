using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Infrastructure
{
    // Classe auxiliar para gerenciamento de tokens OAuth no Bling API
    public class OAuthHelperGetTokens
    {
        // Credenciais do cliente (clientId e clientSecret) registradas no sistema OAuth do Bling.
        // Estas credenciais devem ser armazenadas de forma segura, por exemplo, em variáveis de ambiente.
        private static readonly string clientId = "95816cb9fddf7d246bf47504382c7de6267abd33";
        private static readonly string clientSecret = "ddc1be11fb1799568f00770c28c6ed811c3d1a2051ae8d3894aa023a5e95";
        
        // URL de redirecionamento registrada no sistema OAuth do Bling.
        // O Bling redirecionará o usuário para essa URL após a autorização.
        private static readonly string redirectUri = "http://localhost:3000/callback";

        // URL do endpoint de token do Bling API.
        // Essa URL é usada para obter e atualizar tokens de acesso.
        private static readonly string tokenUrl = "https://bling.com.br/Api/v3/oauth/token";

        // Método para obter um access token utilizando um código de autorização (authorization code).
        public static async Task<string> GetAccessTokenAsync(string authorizationCode)
        {
            // Criação de um cliente HTTP para fazer a solicitação
            using (HttpClient client = new HttpClient())
            {
                // Criação do cabeçalho de autorização em formato Basic (Base64 codificado com clientId e clientSecret)
                var authValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + authValue);

                // Dados necessários para a solicitação do token
                var requestData = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", authorizationCode),
                new KeyValuePair<string, string>("redirect_uri", redirectUri)
                });

                // Envio da requisição POST para a URL de token
                var response = await client.PostAsync(tokenUrl, requestData);

                // Leitura da resposta como string
                var responseContent = await response.Content.ReadAsStringAsync();

                // Exibição do token recebido no console para fins de depuração
                Console.WriteLine("Token: " + responseContent);

                // Retorno do conteúdo da resposta (contém o access token em formato JSON)
                return responseContent; //responseContent Retorna a resposta com o access_token
            }
        }
        // Método para atualizar um access token utilizando um refresh token
        public static async Task<string> RefreshAccessTokenAsync(string refreshToken)
        {
            // Criação de um cliente HTTP para fazer a solicitação
            using (HttpClient client = new HttpClient())
            {
                // Criação do cabeçalho de autorização em formato Basic (Base64 codificado com clientId e clientSecret)
                var authValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + authValue);

                // Dados necessários para a solicitação do refresh token
                var requestData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("refresh_token", refreshToken)
                });

                // Envio da requisição POST para a URL de token
                var response = await client.PostAsync(tokenUrl, requestData);

                // Leitura da resposta como string
                var responseContent = await response.Content.ReadAsStringAsync();

                // Exibição do novo token recebido no console para fins de depuração
                Console.WriteLine("Refresh_Token: " + responseContent);

                // Retorno do conteúdo da resposta (contém o novo access token em formato JSON)
                return responseContent; // Retorna a resposta com o novo access_token
            }
        }
    }
}
