using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Entities.XMLEntities;
using BlingApiDailyConsult.Infrastructure;
using BlingApiDailyConsult.Interfaces;
using BlingApiDailyConsult.Repository;
using BlingApiDailyConsult.Tests;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Org.BouncyCastle.Crypto;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BlingApiDailyConsult
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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


            /*
            // Teste request e gravação no BD - pedidos de venda
            try
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
            }
            */


            /*
            // Teste request e gravação no BD - Produtos
            try
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
            }
            */


            /*
            // Teste request e gravação no BD - itens dos pedidos de venda
            try
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
            }
            */

            /*
            // Teste request insert no BD - Pedido Compra
            try
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
            }

            // Test request e insert no BD - itens pedido compra
            try
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
             }
            */


            /*
            // Teste request e insert no BD - Notas Fiscais
            try 
            { 
                var testeNotaFiscal = new NotaFiscalFetcherTest(tokenManager, configuration);
                await testeNotaFiscal.TestReqInsertNotaFiscal();                
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
            */


            /*
            // Testes com XML
            /*NotaFiscalRepository nfeRep = new NotaFiscalRepository(configuration);

            IEnumerable<string> listXml = await nfeRep.GetXmlLink();

            foreach (string str in listXml)
            {
                Console.WriteLine(str);
                Console.WriteLine();
                NfeProc nfeProc = await BlingXMLFetcher.GetXML(str);
                Console.WriteLine(nfeProc.NFe.InfNFe.Total.ICMSTot.VTotTrib);
                Console.WriteLine();
            }
            */


            /*
            // Obter histórico dos produtos por Id de produto
            try
            {
                var produtoHistoricoTest = new ProdutoHistoricoTest(configuration);
                await produtoHistoricoTest.TestReqInsertHistoricoProduto();               
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
            /*
            

            /*
            var prodHistoricoRepository = new ProdutoHistoricoRepository(configuration);
            
            var produtoHistorico = new BlingProdutoHistoricoFetcher();

            var produtoIdAndName = new ProdutoRepository(configuration);

            List<string> prodFaltante = await prodHistoricoRepository.GetMissingProdutos();

            Dictionary<string, string> dict = await produtoIdAndName.GetIdsAndNamesAsync();

            IEnumerable<RegistroProdutoEstoque> registros = new List<RegistroProdutoEstoque>();

            foreach (var produto in prodFaltante)
            {
                Console.WriteLine(produto);

                registros = produtoHistorico.GetRegistroProdutoEstoque(produto);

                if (dict.Keys.Contains(produto))
                {
                    foreach (var registro in registros)
                    {
                        Console.WriteLine(produto + ", " + dict[produto] + ", " + registro.Data);
                        await prodHistoricoRepository.Add(produto, dict[produto], registro);
                    }
                }
            }
            

            Dictionary<string, Tuple<string, DateTime>> dict =  await prodHistoricoRepository.GetIdNameDateAsync("15971597021");

            int counter = 0;

            foreach (var key in dict)
            {
                Console.WriteLine($"{key.Key}, {key.Value.Item1}, {key.Value.Item2}");
                counter++;
            }

            Console.WriteLine(counter);

            */

            stopwatch.Stop();
            Console.WriteLine($"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms");

            // Converte milissegundos para minutos
            double minutos = stopwatch.ElapsedMilliseconds / 60000.0;

            // Exibe o resultado
            Console.WriteLine($"Tempo em minutos: {minutos} minutos");
            

        }
    }
}

