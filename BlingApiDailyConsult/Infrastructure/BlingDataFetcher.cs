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
        // URL da API Bling com os parâmetros para consulta
        private const string ApiUrl = "https://api.bling.com.br/Api/v3/pedidos/vendas?pagina=1&limite=100&dataInicial=2024-09-01&dataFinal=2024-09-30";

        // String de conexão com o banco de dados MySQL
        private readonly string ConnectionString;

        public BlingDataFetcher(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Método assíncrono que busca dados da API e armazena no banco de dados
        public async Task FetchAndStoreData()
        {
            // Obtém os pedidos da API
            var pedidos = await ObterPedidosDaApi(ApiUrl);

            // Cria uma conexão com o banco de dados
            using (var conn = new MySqlConnection(ConnectionString))
            {
                // Abre a conexão com o banco de dados
                conn.Open();

                // Para cada pedido obtido da API, insere no banco de dados
                foreach (var pedido in pedidos)
                {
                    InserirPedidoNoBanco(pedido, conn);
                }
            }
        }

        // Método para obter pedidos da API Bling
        private async Task<Pedido[]> ObterPedidosDaApi(string url)
        {
            // Cria uma instância do HttpClient para fazer a requisição HTTP
            using (HttpClient client = new HttpClient())
            {
                // Adiciona o cabeçalho de autorização com o token de acesso
                client.DefaultRequestHeaders.Add("Authorization", "Bearer 8fff4d7369eeafba272958db2801b2f2e8150fc9");

                // Realiza a requisição GET para a API
                HttpResponseMessage response = await client.GetAsync(url);

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

        // Método para inserir um pedido no banco de dados
        private void InserirPedidoNoBanco(Pedido pedido, MySqlConnection conn)
        {
            // Comando SQL para inserir os dados do pedido na tabela de pedidos do banco
            string sql = @"
                INSERT INTO pedidos 
                    (id, numero, data, total_produtos, status_id, status_valor, cliente_id, cliente_nome) 
                VALUES 
                    (@id, @numero, @data, @total_produtos, @status_id, @status_valor, @cliente_id, @cliente_nome)
                ON DUPLICATE KEY UPDATE 
                    numero = VALUES(numero),
                    data = VALUES(data),
                    total_produtos = VALUES(total_produtos),
                    status_id = VALUES(status_id),
                    status_valor = VALUES(status_valor),
                    cliente_id = VALUES(cliente_id),
                    cliente_nome = VALUES(cliente_nome);";

            // Cria um comando SQL com o comando definido acima e a conexão com o banco
            using (var cmd = new MySqlCommand(sql, conn))
            {
                // Adiciona os parâmetros ao comando SQL para evitar SQL Injection
                cmd.Parameters.AddWithValue("@id", pedido.Id);
                cmd.Parameters.AddWithValue("@numero", pedido.Numero);
                cmd.Parameters.AddWithValue("@data", pedido.Data);
                cmd.Parameters.AddWithValue("@total_produtos", pedido.TotalProdutos);
                cmd.Parameters.AddWithValue("@status_id", pedido.Situacao.Id);
                cmd.Parameters.AddWithValue("@status_valor", pedido.Situacao.Valor);
                cmd.Parameters.AddWithValue("@cliente_id", pedido.Contato.Id);
                cmd.Parameters.AddWithValue("@cliente_nome", pedido.Contato.Nome);

                // Executa o comando no banco de dados
                cmd.ExecuteNonQuery();
            }
        }
    }
}
