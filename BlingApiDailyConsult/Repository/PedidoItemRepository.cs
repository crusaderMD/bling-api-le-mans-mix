using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Infrastructure;
using BlingApiDailyConsult.Interfaces;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Repository
{

    internal class PedidoItemRepository : IRepository<Dictionary<string, List<Item>>>
    {
        private readonly string? _connectionString;        
        private readonly BlingSingleProdutoFetcher _blingSingleProdutoFetcher;
        private readonly ProdutoRepository _produtoRepository;

        public PedidoItemRepository(IConfiguration configuration, TokenManager tokenManager)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _blingSingleProdutoFetcher = new BlingSingleProdutoFetcher(tokenManager);
            _produtoRepository = new ProdutoRepository(configuration);
        }

        // Salva os itens de um pedido no BD
        public void Add(Dictionary<string, List<Item>> pedidos)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    foreach (var pedido in pedidos)
                    {
                        // Começa uma transação para cada chave (pedido)
                        using (var transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                foreach (var item in pedido.Value)
                                {
                                    // Apenas para debugar
                                    Console.WriteLine(this + $" Pedido: {pedido.Key}, Produto: {item?.Produto?.Id}");

                                    // Inserção ou atualização do item
                                    InsertOrUpdatePedidoitem(pedido.Key, item, conn, transaction);
                                }

                                // Se não houver erro, realiza o commit da transação para esta chave (pedido)
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                // Em caso de erro, faz o rollback apenas da transação atual (para este pedido)
                                transaction.Rollback();
                                Console.WriteLine($"Erro ao processar pedido {pedido.Key}: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Erro ao inserir ou atualizar os itens do pedido no banco de dados: {ex.Message}", ex);
            }
            // Apagar depois
            Console.WriteLine("Pedidos Inseridos com sucesso!");
        }

        // Método auxiliar para inserir ou atualizar os itens de um pedido no BD
        private void InsertOrUpdatePedidoitem(string key, Item? item, MySqlConnection conn, MySqlTransaction transaction)
        {
            try
            {
                Task task = VerifyAndInsertProduto(item.Produto.Id, conn);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                Console.WriteLine($"Detalhes: {ex.StackTrace}");
            }

            // Comando SQL para inserir os dados dos itens do pedido na tabela de pedidos do banco
            string sql = @"INSERT INTO itens_do_pedido
                            (pedido_id, produto_id, nome_produto, quantidade, unidade_produto, preco_produto)
                        VALUES
                            (@pedido_id, @produto_id, @nome_produto, @quantidade, @unidade_produto, @preco_produto)
                        ON DUPLICATE KEY UPDATE
                            produto_id = VALUES(produto_id),
                            nome_produto = VALUES(nome_produto),
                            quantidade = VALUES(quantidade),
                            unidade_produto = VALUES(unidade_produto),
                            preco_produto = VALUES(preco_produto);";
            // Cria um comando SQL com o comando definido acima e a conexão com o banco de dados
            using (var cmd = new MySqlCommand(sql, conn, transaction))
            {
                // Adiciona os parâmetros ao comando SQL para evitar SQL Injection
                cmd.Parameters.AddWithValue("@pedido_id", key);
                cmd.Parameters.AddWithValue("@produto_id", item?.Produto?.Id);
                cmd.Parameters.AddWithValue("@nome_produto", item?.Descricao);
                cmd.Parameters.AddWithValue("@quantidade", item?.Quantidade);
                cmd.Parameters.AddWithValue("@unidade_produto", item?.Unidade);
                cmd.Parameters.AddWithValue("@preco_produto", item?.Valor);

                // Executa o comando no banco de dados
                int returned = cmd.ExecuteNonQuery();

                if (returned > 0)
                {
                    Console.WriteLine($"Operação de gravação do pedido: {key}, produto: {item?.Produto?.Id} concluída com exíto!");
                }
                else
                {
                    Console.WriteLine($"Pedido: {key}, Produto: {item?.Produto?.Id} não pode ser gravado!");
                }
            }
        }

        // Método auxiliar para verificar se um pedido já existe no BD
        private async Task VerifyAndInsertProduto(long produtoId, MySqlConnection conn)
        {
            try
            {
                if (!ProdutoExist(produtoId, conn))
                {    
                    var produto = await _blingSingleProdutoFetcher?.GetSingleProduto(produtoId);

                    if (produto == null)
                    {
                        Console.WriteLine(this + $"Produto {produtoId} não encontrado na API!");
                    }

                    Console.WriteLine(this + $"Produto {produtoId} recuperado com sucesso.");

                    List<Produto>? produtos = new List<Produto>();
                    produtos.Add(produto);
                    _produtoRepository?.Add(produtos);
                }
                else
                {
                    Console.WriteLine(this + $" Produto {produtoId} já existe no banco de dados.");
                }
            }
            catch (Exception ex)
            {
                // Caso ocorra algum erro ao recuperar ou inserir o produto
                Console.WriteLine($"Erro ao verificar ou inserir produto {produtoId}: {ex.Message}");
                // lançar ou re-throw a exceção
            }
        }

        // Método auxilar, consulta se um produto existe na tabela produtos no BD
        private static bool ProdutoExist(long ProdutoId, MySqlConnection conn)
        {
            try
            {
                string sql = @"SELECT COUNT(*) FROM produtos WHERE id = @id";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", ProdutoId);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar a existência do produto: {ex.Message}");
                return false; // Considera falso se ocorrer um erro
            }
        }

        public void Delete(Dictionary<string, List<Item>> entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetAllIdsAsync()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<Item>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Dictionary<string, List<Item>> entity)
        {
            throw new NotImplementedException();
        }
    }
}
