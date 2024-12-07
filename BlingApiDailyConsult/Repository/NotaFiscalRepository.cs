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
    internal class NotaFiscalRepository : IRepository<IEnumerable<NotaFiscal>>
    {
        private readonly string? _connectionString;

        public NotaFiscalRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Add(IEnumerable<NotaFiscal> notasFiscais)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                foreach (var notaFiscal in notasFiscais)
                {
                    try
                    {
                        InsertOrUpdateNotaFiscal(notaFiscal, conn);
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Erro ao processar nota fiscal ID: {notaFiscal.Id}. Erro: {ex.Message}");
                    }
                }
            }
        }

        private void InsertOrUpdateNotaFiscal(NotaFiscal notaFiscal, MySqlConnection conn)
        {
            string sql = @"INSERT INTO notas_fiscais
                        (id, numero, serie, data_emissao, data_operacao, valor, tipo, situacao, link_xml)
                        VALUES
                        (@id, @numero, @serie, @data_emissao, @data_operacao, @valor, @tipo, @situacao, @link_xml)
                        ON DUPLICATE KEY UPDATE
                        numero = VALUES(numero),
                        serie = VALUES(serie),
                        data_emissao = VALUES(data_emissao), 
                        data_operacao = VALUES(data_operacao), 
                        valor = VALUES(valor), 
                        tipo = VALUES(tipo), 
                        situacao = VALUES(situacao), 
                        link_xml = VALUES(link_xml);";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", notaFiscal.Id);
                cmd.Parameters.AddWithValue("@numero", notaFiscal.Numero);
                cmd.Parameters.AddWithValue("@serie", notaFiscal.Serie);
                cmd.Parameters.AddWithValue("@data_emissao", notaFiscal.DataEmissao);
                cmd.Parameters.AddWithValue("@data_operacao", notaFiscal.DataOperacao);
                cmd.Parameters.AddWithValue("@valor", notaFiscal.ValorNota);
                cmd.Parameters.AddWithValue("@tipo", notaFiscal.TipoNotaFiscal);
                cmd.Parameters.AddWithValue("@situacao", notaFiscal.SituacaoNotaFiscal);
                cmd.Parameters.AddWithValue("@link_xml", notaFiscal.LinkXml);

                int returned = cmd.ExecuteNonQuery();

                if (returned > 0)
                {
                    Console.WriteLine($"Operação de gravação da nota fiscal: {notaFiscal?.Id} concluída com exíto!");
                }
                else
                {
                    Console.WriteLine($"Nota fiscal: {notaFiscal?.Id} não pode ser gravado!");
                }
            }
        }

        public void Delete(IEnumerable<NotaFiscal> entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetAllIdsAsync()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NotaFiscal> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<NotaFiscal> entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetXmlLink()
        {
            List<string> linksXml = new List<string>();

            try
            {               
                using (var conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string sql = @"SELECT link_xml
                        FROM notas_fiscais;";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                if (reader.GetString("link_xml") != string.Empty)
                                {
                                    linksXml.Add(reader.GetString("link_xml"));
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

            return linksXml;
        }
    }
}
