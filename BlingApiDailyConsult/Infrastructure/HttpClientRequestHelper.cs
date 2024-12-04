using System.Text.Json;

namespace BlingApiDailyConsult.Infrastructure
{
    public class HttpClientRequestHelper
    {
        private readonly TokenManager _tokenManager;

        public HttpClientRequestHelper(TokenManager tokenManager)
        {
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
        }

        public async Task<T> FetchDataAsync<T>(string url)
        {
            // Recebe um token válido
            string validToken = await _tokenManager.GetValidAccessTokenAsync();

            // Cria uma instância do HttpClient para fazer a requisição HTTP
            using var client = new HttpClient();

            // Adiciona o cabeçalho de autorização com o token de acesso
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + validToken);

            // Apagar depois
            Console.WriteLine(this + " " + url);

            // Realiza a requisição GET para a API
            HttpResponseMessage response = await client.GetAsync(url) ?? throw new Exception();

            // apagar depois
            Console.WriteLine(this + " Resposta:");
            Console.WriteLine(response);
            Console.WriteLine();

            // Verifica se a resposta da API foi bem-sucedida
            if (!response.IsSuccessStatusCode)
            {
                // Caso não seja bem-sucedida, lança uma exceção com o código de status
                throw new Exception($"Erro na requisição. Código de status: {response.StatusCode}");
            }

            // Lê a resposta como string
            string jsonResponse = await response.Content.ReadAsStringAsync();

            // Exibe o JSON recebido para depuração
            Console.WriteLine();
            Console.WriteLine(this + " JSON recebido:");
            Console.WriteLine(jsonResponse);
            Console.WriteLine();

            // Verifica se a resposta está vazia ou nula
            if (string.IsNullOrEmpty(jsonResponse))
            {
                throw new Exception("Resposta vazia da API.");
            }

            // Deserializa o JSON para o objeto ApiResponse, que contém a lista de pedidos
            return JsonSerializer.Deserialize<T>(jsonResponse) ?? throw new Exception("Erro ao deserializar a resposta da API.");
        }
    }
}
