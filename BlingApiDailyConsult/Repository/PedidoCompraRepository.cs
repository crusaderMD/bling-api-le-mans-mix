﻿using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Interfaces;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Repository
{
    internal class PedidoCompraRepository : IRepository<IEnumerable<Pedido>>
    {
        private readonly string? _connectionString;

        public PedidoCompraRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Add(IEnumerable<Pedido> pedidos)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    foreach (var pedido in pedidos)
                    {                        
                        InsertOrUpdatePedidoCompra(pedido, conn);                        
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Erro ao inserir ou atualizar o pedido de compra no banco de dados: {ex.Message}", ex);
            }
        }

        private void InsertOrUpdatePedidoCompra(Pedido pedido, MySqlConnection conn)
        {
            // Comando SQL para inserir os dados do pedido na tabela de pedidos_compra no BD
            string sql = @"INSERT INTO pedido_compra
                    (id, numero, data, total_produtos, fornecedor_id, situacao_valor)
                    VALUES
                    (@id, @numero, @data, @total_produtos, @fornecedor_id, @situacao_valor)
                    ON DUPLICATE KEY UPDATE
                    numero = VALUES(numero),
                    data = VALUES(data),
                    total_produtos = VALUES(total_produtos),
                    fornecedor_id = VALUES(fornecedor_id),
                    situacao_valor = VALUES(situacao_valor);";

            // Cria um comando SQL com a string definida acima e a conexão com o banco de dados
            using (var cmd = new MySqlCommand(sql, conn))
            {
                // Adiciona os parâmetros ao comando SQL para evitar SQL Injection
                cmd.Parameters.AddWithValue("@id", pedido.Id);
                cmd.Parameters.AddWithValue("@numero", pedido.Numero);
                cmd.Parameters.AddWithValue("@data", pedido.Data);
                cmd.Parameters.AddWithValue("@total_produtos", pedido.TotalProdutos);
                cmd.Parameters.AddWithValue("@fornecedor_id", pedido?.Fornecedor?.Id);
                cmd.Parameters.AddWithValue("@situacao_valor", pedido?.Situacao?.Valor);

                // Executa o comando no banco de dados
                int returned = cmd.ExecuteNonQuery();

                if (returned > 0)
                {
                    Console.WriteLine($"Operação de gravação do pedido de compra: {pedido?.Id} concluída com exíto!");
                }
                else
                {
                    Console.WriteLine($"Pedido: {pedido?.Id} não pode ser gravado!");
                }
            }
        }

        public void Delete(IEnumerable<Pedido> entity)
        {
            throw new NotImplementedException();
        }       

        public IEnumerable<Pedido> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<Pedido> entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetAllIdsAsync()
        {
            List<string> ids = new List<string>();

            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string sql = @"SELECT id 
                                FROM pedido_compra
                                WHERE id NOT IN (SELECT pedido_id FROM itens_pedido_compra);";

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ids;
        }
    }
}
