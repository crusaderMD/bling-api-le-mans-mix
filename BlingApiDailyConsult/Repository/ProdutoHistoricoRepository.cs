using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Interfaces;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Repository
{
    internal class ProdutoHistoricoRepository
    {
        private readonly string? _connectionString;
        private readonly ProdutoRepository _produtoRepository;

        public ProdutoHistoricoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _produtoRepository = new ProdutoRepository(configuration);
        }

        public async Task Add(string produtoId, string produtoNome, RegistroProdutoEstoque registroLineProduto)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string sql = @"INSERT INTO produtos_historico
                                (Id, nome, data, entrada, saida, preco_venda, preco_compra, preco_custo, observacao, origem, tipo)
                                VALUES
                                (@Id, @nome, @data, @entrada, @saida, @preco_venda, @preco_compra, @preco_custo, @observacao, @origem, @tipo)
                                ON DUPLICATE KEY UPDATE
                                entrada = VALUES(entrada), 
                                saida = VALUES(saida),                                
                                preco_venda = VALUES(preco_venda), 
                                preco_compra = VALUES(preco_compra), 
                                preco_custo = VALUES(preco_custo), 
                                observacao = VALUES(observacao), 
                                origem = VALUES(origem), 
                                tipo = VALUES(tipo)
                               ;";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", produtoId);
                        cmd.Parameters.AddWithValue("@nome", produtoNome);
                        cmd.Parameters.AddWithValue("@data", registroLineProduto.Data);
                        cmd.Parameters.AddWithValue("@entrada", registroLineProduto.Entrada);
                        cmd.Parameters.AddWithValue("@saida", registroLineProduto.Saida);
                        cmd.Parameters.AddWithValue("@preco_venda", registroLineProduto.PrecoVenda);
                        cmd.Parameters.AddWithValue("@preco_compra", registroLineProduto.PrecoCompra);
                        cmd.Parameters.AddWithValue("@preco_custo", registroLineProduto.PrecoCusto);
                        cmd.Parameters.AddWithValue("@observacao", registroLineProduto.Observacao);
                        cmd.Parameters.AddWithValue("@origem", registroLineProduto.Origem);
                        cmd.Parameters.AddWithValue("@tipo", registroLineProduto.Tipo);

                        int returned = await cmd.ExecuteNonQueryAsync();

                        if (returned > 0)
                        {
                            Console.WriteLine($"Operação de gravação do registro do produto: {produtoId} concluída com exíto!");
                        }
                        else
                        {
                            Console.WriteLine($"Produto: {produtoId} não pode ser gravado!");
                        }
                    }
                }
            }
            catch(MySqlException ex)
            {
                Console.WriteLine($"MySql Error: {ex.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Generic Error: {ex.Message}");
            }
        }

        public void Delete(IEnumerable<RegistroProdutoEstoque> entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetAllIdsAsync()
        {
            List<string> produtoIds = new List<string>();

            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string sql = @"SELECT Id
                                FROM produtos_historico;";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {

                                if (reader.GetInt64("id").ToString() != null)
                                {
                                    produtoIds.Add(reader.GetInt64("id").ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erro SQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro genérico: " + ex.Message);
            }
            return produtoIds;
        }

        public IEnumerable<RegistroProdutoEstoque> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<RegistroProdutoEstoque> entity)
        {
            throw new NotImplementedException();
        }

        // Provisório Modificar depois
        public async Task<List<string>> GetMissingProdutos()
        {
            
            IEnumerable<string> prodIds = await _produtoRepository.GetAllIdsAsync();
            IEnumerable<string> prodHistIds = await GetAllIdsAsync();

            List<string> prodFaltante = new List<string>();

            foreach (var id in prodIds)
            {
                if (!prodHistIds.Contains(id))
                {
                    prodFaltante.Add(id);
                }
            }

            return prodFaltante;
        }

        public async Task<Dictionary<string, Tuple<string, DateTime>>> GetIdNameDateAsync(string id)
        {
            Dictionary<string, Tuple<string, DateTime>> dict = new Dictionary<string, Tuple<string, DateTime>>();

            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string sql = @$"WITH cte AS (
                            SELECT 
                                id,
                                MAX(data) AS data_mais_recente
                            FROM produtos_historico
                            GROUP BY id
                            )
                            SELECT ph.id, ph.nome, ph.data
                            FROM produtos_historico ph
                            JOIN cte ON ph.id = cte.id AND ph.data = cte.data_mais_recente
                            WHERE ph.id = {id};";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {

                                if (reader.GetInt64("id").ToString() != null)
                                {
                                    dict.Add(reader.GetInt64("Id").ToString(), Tuple.Create(reader.GetString("nome"), reader.GetDateTime("data")));
                                }
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erro SQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro genérico: " + ex.Message);
            }
            return dict;
        }
    }
}
