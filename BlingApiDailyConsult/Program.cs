using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Infrastructure;
using BlingApiDailyConsult.Interfaces;
using BlingApiDailyConsult.Repository;
using BlingApiDailyConsult.Tests;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading.Tasks;

namespace BlingApiDailyConsult
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Instancia o DataBaseHelper com a configuração carregada
            var dataBaseHelper = new DataBaseHelper(configuration);

            // Instancia o TokenManager injetando o DataBaseHelper
            var tokenManager = new TokenManager(dataBaseHelper);

            // Teste autenticação
            //OAuthHelperGetAuthCode.RedirectToAuthUrl();



            // Teste request e gravação no BD - pedidos de venda
            /*try
            {
                var testPedidovenda = new PedidoVendaFetcherTest(tokenManager, configuration);
                await testPedidovenda.TesteReqInsertPedidoVenda();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Http error: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro de Argumento: {ex.Message}");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro SQL: {ex.Message}");
            }                        
            catch (Exception ex)
            {
                Console.WriteLine($"Erro genérico: {ex.Message}");
            }*/



            // Teste request e gravação no BD - Produtos
            /*try
            {
                var testProduto = new ProdutoFetcherTest(tokenManager, configuration);
                await testProduto.TestReqInsertProduto();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Http error: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro de Argumento: {ex.Message}");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro genérico: {ex.Message}");
            }*/



            // Teste request e gravação no BD - itens dos pedidos de venda
            /*try
            {
                var testItensVenda = new PedidoVendaItemTest(tokenManager, configuration);
                await testItensVenda.TestReqInsertVendaItem();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Http error: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro de Argumento: {ex.Message}");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro genérico: {ex.Message}");
            }*/



            // Teste request insert no BD - Pedido Compra
            /*try
            {
                var testPedidoCompra = new PedidoCompraFetcherTest(tokenManager, configuration);
                await testPedidoCompra.TestReqInsertPedidoCompra();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Http error: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro de Argumento: {ex.Message}");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro genérico: {ex.Message}");
            }*/



            // Test request e insert no BD - itens pedido compra
            /*try
             {
                 var testItemPedidoCompra = new PedidoCompraItemTest(tokenManager, configuration);
                 await testItemPedidoCompra.TestReqInsertItemPedidoCompra();      
             }
             catch (HttpRequestException ex)
             {
                 Console.WriteLine($"Http error: {ex.Message}");
             }
             catch (ArgumentException ex)
             {
                 Console.WriteLine($"Erro de Argumento: {ex.Message}");
             }
             catch (MySqlException ex)
             {
                 Console.WriteLine($"Erro SQL: {ex.Message}");
             }
             catch (Exception ex)
             {
                 Console.WriteLine($"Erro genérico: {ex.Message}");
             }*/

            try 
            { 
                var testeNotaFiscal = new NotaFiscalFetcherTest(tokenManager, configuration);
                await testeNotaFiscal.TestReqInsertNotaFiscal();

                /*var notasFiscais = new BlingNotaFiscalFetcher(tokenManager);                
                var notaFiscal = new BlingSingleNotaFiscalFetcher(tokenManager);
                var nfeRep = new NotaFiscalRepository(configuration);

                NotaFiscal[] notas = await notasFiscais.ExecuteAsync();
                List<NotaFiscal> list = new List<NotaFiscal>();

                foreach (var nota in notas)
                {
                    var retorno = await notaFiscal.GetSingleNotaFiscal(nota.Id);                    
                    list.Add(retorno);
                    nfeRep.Add(list);
                    await Task.Delay(500); // realiza 2 requisições por segundo
                }*/
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Http error: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro de Argumento: {ex.Message}");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro genérico: {ex.Message}");
            }            
        }
    }
}

