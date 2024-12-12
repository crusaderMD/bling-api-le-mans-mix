using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Interfaces;
using BlingApiDailyConsult.Services;
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
    internal class PedidoVendaRepository : IRepository<IEnumerable<Pedido>>
    {
        private readonly string? _connectionString;

        public PedidoVendaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Salva os pedidos (sem os itens) no BD
        public void Add(IEnumerable<Pedido> pedidos)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    foreach (var pedido in pedidos)
                    {
                        InsertOrUpdatePedidoVenda(pedido, conn);
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Erro ao inserir ou atualizar o pedido de venda no banco de dados: {ex.Message}", ex);
            }
            
        }

        // Método auxiliar para salvar ou atualizar os pedidos no BD
        private void InsertOrUpdatePedidoVenda(Pedido pedido, MySqlConnection conn)
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

            // Cria um comando SQL com a string definida acima e a conexão com o banco de dados
            using (var cmd = new MySqlCommand(sql, conn))
            {
                // Adiciona os parâmetros ao comando SQL para evitar SQL Injection
                cmd.Parameters.AddWithValue("@id", pedido.Id);
                cmd.Parameters.AddWithValue("@numero", pedido.Numero);
                cmd.Parameters.AddWithValue("@data", pedido.Data);
                cmd.Parameters.AddWithValue("@total_produtos", pedido.TotalProdutos);
                cmd.Parameters.AddWithValue("@status_id", pedido?.Situacao?.Id);
                cmd.Parameters.AddWithValue("@status_valor", pedido?.Situacao?.Valor);
                cmd.Parameters.AddWithValue("@cliente_id", pedido?.Contato?.Id);
                cmd.Parameters.AddWithValue("@cliente_nome", pedido?.Contato?.Nome);

                // Executa o comando no banco de dados
                int returned = cmd.ExecuteNonQuery();

                if (returned > 0)
                {
                    Console.WriteLine($"Operação de gravação do pedido de venda: {pedido?.Id} concluída com exíto!");
                }
                else
                {
                    Console.WriteLine($"Pedido: {pedido?.Id} não pode ser gravado!");
                }
            }
        }

        public void Delete( IEnumerable<Pedido> pedidos)
        {
            throw new NotImplementedException();
        }        

        public IEnumerable<Pedido> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update( IEnumerable<Pedido> pedidos )
        {
            throw new NotImplementedException();
        }

        // Obtem os Ids dos pedidos salvos no BD
        public async Task<IEnumerable<string>> GetAllIdsAsync()
        {
            DateRequestHelper dateRequestRelper = new();

            string dataInicial = dateRequestRelper.GetStartDate();
            string dataFinal = dateRequestRelper.GetEndDate();

            List<string> ids = new List<string>();

            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string sql = @$"SELECT id 
                                FROM pedidos                                
                                WHERE data BETWEEN '{dataInicial}'
                                AND '{dataFinal}';";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                ids.Add(reader.GetInt64("id").ToString());
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
            return ids;
        }
    }
}
