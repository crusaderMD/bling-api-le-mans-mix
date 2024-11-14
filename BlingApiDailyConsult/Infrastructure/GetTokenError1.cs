using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Infrastructure
{
    internal class GetTokenError1
    {
        private static readonly HttpClient client = new HttpClient();
        private string currentToken = "8fff4d7369eeafba272958db2801b2f2e8150fc9"; // Token atual (exemplo)
        private DateTime tokenExpirationTime; // Data e hora da expiração do token

        // Método para obter um novo token
        private async Task<string> GetNewTokenAsync()
        {
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.bling.com.br/oauth/token");
            // Adicione os parâmetros necessários para a requisição de token, como client_id, client_secret, etc.
            // Exemplo de payload para o OAuth2:
            tokenRequest.Content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("grant_type", "code"),
            new KeyValuePair<string, string>("client_id", "95816cb9fddf7d246bf47504382c7de6267abd33"),
            new KeyValuePair<string, string>("client_secret", "ddc1be11fb1799568f00770c28c6ed811c3d1a2051ae8d3894aa023a5e95")
        });

            var response = await client.SendAsync(tokenRequest);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            Console.WriteLine(jsonResponse);

            // Parse o JSON para obter o novo token
            var token = ExtractTokenFromJson(jsonResponse); // Assumindo que você tem um método para extrair o token do JSON
            tokenExpirationTime = DateTime.UtcNow.AddMinutes(60); // Definindo o tempo de expiração do token (exemplo de 1 hora)

            return token;
        }

        // Método para verificar se o token precisa ser renovado
        private async Task<string> GetValidTokenAsync()
        {
            if (DateTime.UtcNow >= tokenExpirationTime)
            {
                // O token expirou, obtenha um novo token
                currentToken = await GetNewTokenAsync();
            }
            return currentToken;
        }

        // Método para configurar o cabeçalho de autorização
        private async Task SetAuthorizationHeaderAsync()
        {
            var token = await GetValidTokenAsync();
            client.DefaultRequestHeaders.Remove("Authorization"); // Remover o token antigo
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        // Método para buscar os pedidos da API
        public async Task FetchOrders()
        {
            await SetAuthorizationHeaderAsync(); // Atualiza o cabeçalho com o token atual

            var response = await client.GetAsync("https://api.bling.com.br/api/pedidos"); // Exemplo de requisição para obter pedidos
            response.EnsureSuccessStatusCode();

            // Aqui você processa a resposta da API
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
        }

        // Método para extrair o token do JSON (simplificado)
        private string ExtractTokenFromJson(string jsonResponse)
        {
            // Parse o JSON e extraia o token. Este é um exemplo simplificado.
            // Use uma biblioteca como Newtonsoft.Json ou System.Text.Json para extrair o valor do token
            var tokenObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonResponse);
            return tokenObject.access_token;
        }
    }

}

