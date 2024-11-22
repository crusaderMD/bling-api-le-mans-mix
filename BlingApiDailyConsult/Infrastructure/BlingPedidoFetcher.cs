using System.Text.Json;
using MySql.Data.MySqlClient;
using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult;
using Microsoft.Extensions.Configuration;
using BlingApiDailyConsult.Interfaces;
using BlingApiDailyConsult.Services;
using System.Security.Policy;

// Namespace responsável pelas interações com a API Bling e o banco de dados
namespace BlingApiDailyConsult.Infrastructure
{
    // Classe responsável por buscar os pedidos da API Bling e armazená-los no banco de dados
    internal class BlingPedidoFetcher : IBlingApiFetcher<Pedido[]>
    {
        // URL da API Bling com os parâmetros para consulta de pedidos por periodo
        private const string baseUrl = "https://api.bling.com.br/Api/v3/pedidos/vendas?";

        // String de conexão com o banco de dados MySQL
        private readonly TokenManager _tokenManager;

        public BlingPedidoFetcher(TokenManager tokenManager)
        {
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager)); 
        }

        // Método para obter pedidos da API Bling
        public async Task<Pedido[]> ExecuteAsync()
        {
            // Instancia a classe PaginationHelper que auxilia na iteração das paginas
            var paginationHelper = new PaginationHelper();

            // Instancia a classe DateRequestHelper
            var dateRequestHelper = new DateRequestHelper();

            string dateFilteredUrl = baseUrl + dateRequestHelper.GetDateQueryString();

            return (await paginationHelper.FetchAllPagesAsync<Pedido>(dateFilteredUrl, async (paginatedUrl) =>
            {
                // Recebe um token válido
                string validToken = await _tokenManager.GetValidAccessTokenAsync();

                // Cria uma instância do HttpClient para fazer a requisição HTTP
                using var client = new HttpClient();

                // Adiciona o cabeçalho de autorização com o token de acesso
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + validToken);

                // Realiza a requisição GET para a API
                HttpResponseMessage response = await client.GetAsync(paginatedUrl);

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
                var apiPedidoResponse = JsonSerializer.Deserialize<ApiResponse<Pedido>>(jsonResponse);                

                // Retorna a lista de pedidos deserializada
                return apiPedidoResponse?.Data?.ToList() ?? new List<Pedido>();               
            }
            )).ToArray();
        }        
    }
}
