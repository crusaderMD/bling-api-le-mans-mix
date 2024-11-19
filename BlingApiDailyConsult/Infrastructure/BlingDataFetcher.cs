using System.Text.Json;
using MySql.Data.MySqlClient;
using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult;
using Microsoft.Extensions.Configuration;

// Namespace responsável pelas interações com a API Bling e o banco de dados
namespace BlingApiDailyConsult.Infrastructure
{
    // Classe responsável por buscar os pedidos da API Bling e armazená-los no banco de dados
    internal class BlingDataFetcher
    {
        // URL da API Bling com os parâmetros para consulta de pedidos por periodo
        private const string ApiUrl = "https://api.bling.com.br/Api/v3/pedidos/vendas?pagina=1&limite=100&dataInicial=2024-01-01&dataFinal=2024-11-20";

        // String de conexão com o banco de dados MySQL
        private readonly TokenManager _tokenManager;

        public BlingDataFetcher(TokenManager tokenManager)
        {
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager)); 
        }

        // Método para obter pedidos da API Bling
        public async Task<Pedido[]> FetchPedidosAsync()
        {
            // Recebe um token válido
            string validToken = await _tokenManager.GetValidAccessTokenAsync();

            // Cria uma instância do HttpClient para fazer a requisição HTTP
            using (HttpClient client = new HttpClient())
            {
                // Adiciona o cabeçalho de autorização com o token de acesso
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + validToken);

                // Realiza a requisição GET para a API
                HttpResponseMessage response = await client.GetAsync(ApiUrl);

                // Verifica se a resposta da API foi bem-sucedida
                if (!response.IsSuccessStatusCode)
                {
                    // Caso não seja bem-sucedida, lança uma exceção com o código de status
                    throw new Exception($"Erro na requisição. Código de status: {response.StatusCode}");
                }

                // Lê a resposta como string
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Exibe o JSON recebido para depuração
                Console.WriteLine("JSON recebido:");
                Console.WriteLine(jsonResponse);

                // Verifica se a resposta está vazia ou nula
                if (string.IsNullOrEmpty(jsonResponse))
                {
                    throw new Exception("Resposta vazia da API.");
                }

                // Deserializa o JSON para o objeto ApiResponse, que contém a lista de pedidos
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonResponse);

                // Verifica se os pedidos foram encontrados na resposta da API
                if (apiResponse?.Pedidos == null || !apiResponse.Pedidos.Any())
                {
                    throw new Exception("Pedidos não encontrados na resposta da API.");
                }

                // Retorna a lista de pedidos deserializada
                return apiResponse.Pedidos.ToArray();
            }
        }        
    }
}
