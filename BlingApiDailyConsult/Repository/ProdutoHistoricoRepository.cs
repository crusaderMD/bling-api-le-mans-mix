using BlingApiDailyConsult.Entities;
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
    internal class ProdutoHistoricoRepository
    {
        private readonly string? _connectionString;

        public ProdutoHistoricoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
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

        public Task<IEnumerable<string>> GetAllIdsAsync()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RegistroProdutoEstoque> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<RegistroProdutoEstoque> entity)
        {
            throw new NotImplementedException();
        }
    }
}
