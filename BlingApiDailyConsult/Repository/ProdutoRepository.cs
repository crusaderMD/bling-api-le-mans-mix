using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Interfaces;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Repository
{
    internal class ProdutoRepository : IRepository<IEnumerable<Produto>>
    {
        private readonly string? _connectionString;

        public ProdutoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Salva os produtos no BD
        public void Add(IEnumerable<Produto> produtos)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    foreach (var produto in produtos)
                    {
                        InsertOrUpdateProduto(produto, conn);                        
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Erro ao inserir ou atualizar o produto no banco de dados: {ex.Message}", ex);
            }
        }

        // Método auxiliar para salvar ou atualizar os produtos no BD
        private void InsertOrUpdateProduto(Produto produto, MySqlConnection conn)
        {
            // Comando SQL para inserir os dados do produto na tabela de produtos do banco de dados
            string sql = @"
                INSERT INTO produtos 
                    (id, nome, codigo, preco, preco_custo, saldo_total_estoque, tipo, situacao, formato) 
                VALUES 
                    (@id, @nome, @codigo, @preco, @preco_custo, @saldo_total_estoque, @tipo, @situacao, @formato)
                ON DUPLICATE KEY UPDATE 
                    nome = VALUES(nome),
                    codigo = VALUES(codigo),
                    preco = VALUES(preco),
                    preco_custo = VALUES(preco_custo),
                    saldo_total_estoque = VALUES(saldo_total_estoque),
                    tipo = VALUES(tipo),
                    situacao = VALUES(situacao),
                    formato = VALUES(formato);";

            // Cria um comando SQL com o comando definido acima e a conexão com o banco
            using (var cmd = new MySqlCommand(sql, conn))
            {
                // Adiciona os parâmetros ao comando SQL para evitar SQL Injection
                cmd.Parameters.AddWithValue("@id", produto.Id);
                cmd.Parameters.AddWithValue("@nome", produto.Nome);
                cmd.Parameters.AddWithValue("@codigo", produto.Codigo);
                cmd.Parameters.AddWithValue("@preco", produto.Preco);
                cmd.Parameters.AddWithValue("@preco_custo", produto.PrecoCusto);
                cmd.Parameters.AddWithValue("@saldo_total_estoque", produto.Estoque?.SaldoTotal);
                cmd.Parameters.AddWithValue("@tipo", produto.Tipo);
                cmd.Parameters.AddWithValue("@situacao", produto.Situacao);
                cmd.Parameters.AddWithValue("@formato", produto.Formato);

                // Executa o comando no banco de dados
                int returned = cmd.ExecuteNonQuery();

                if (returned > 0)
                {
                    Console.WriteLine($"Operação de gravação do produto: {produto?.Id} concluída com exíto!");
                }
                else
                {
                    Console.WriteLine($"Prosuto: {produto?.Id} não pode ser gravado!");
                }
            }
        }

        public void Update(IEnumerable<Produto> produtos)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<Produto> produtos)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Produto> GetById(int id)
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
                                FROM produtos;";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync()){

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
    }
}
