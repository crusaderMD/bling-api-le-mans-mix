using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Infrastructure
{
    public class OAuthHelperGetTokens
    {  
        // verificar a necessidade de implementar UI para informar
        private static readonly string clientId = "95816cb9fddf7d246bf47504382c7de6267abd33";
        // verificar a necessidade de implementar UI para informar
        private static readonly string clientSecret = "ddc1be11fb1799568f00770c28c6ed811c3d1a2051ae8d3894aa023a5e95";
        // verificar a necessidade de implementar UI para informar
        private static readonly string redirectUri = "http://localhost:3000/callback";
        private static readonly string tokenUrl = "https://bling.com.br/Api/v3/oauth/token";

        public static async Task<string> GetAccessTokenAsync(string authorizationCode)
        {
            using (HttpClient client = new HttpClient())
            {
                var authValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + authValue);

                var requestData = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", authorizationCode),
                new KeyValuePair<string, string>("redirect_uri", redirectUri)
                });

                var response = await client.PostAsync(tokenUrl, requestData);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Token: " + responseContent);
                return responseContent; //responseContent Retorna a resposta com o access_token
            }
        }
        public static async Task<string> RefreshAccessTokenAsync(string refreshToken)
        {
            using (HttpClient client = new HttpClient())
            {
                var authValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + authValue);

                var requestData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("refresh_token", refreshToken)
                });

                var response = await client.PostAsync(tokenUrl, requestData);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Refresh_Token: " + responseContent);
                return responseContent; // Retorna a resposta com o novo access_token
            }
        }
    }
}
