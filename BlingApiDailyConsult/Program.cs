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



            // Obter histórico dos produtos por Id de produto
            try
            {
                //var produtoHistoricoTest = new ProdutoHistoricoTest(configuration);
                //await produtoHistoricoTest.TestReqInsertHistoricoProduto();

                BlingProdutoFetcher _blingProdutoFetcher = new BlingProdutoFetcher(tokenManager);
                ProdutoRepository _produtoRepository = new ProdutoRepository(configuration);
                ProdutoHistoricoRepository _produtoHistoricoRepository = new ProdutoHistoricoRepository(configuration);

                Dictionary<string, string> produtoIdsAndNameList = await _produtoRepository.GetIdsAndNamesAsync();

                foreach (var idsAndNames in produtoIdsAndNameList)
                {
                    Console.WriteLine(idsAndNames);
                }

                Dictionary<string, string> idsResults = new Dictionary<string, string>();

                List<string> ids = new List<string>()
            {
                
                "16115202063",
                "16115444379",
                "16115460990",
                "16115469287",
                "16115472469",
                "16115480098",
                "16115583846",
                "16115586218",
                "16115599460",
                "16115615378",
                "16115618414",
                "16116409177",
                "16116413251",
                "16117525576",
                "16117557330",
                "16117597092",
                "16119709654",
                "16120329283",
                "16120349675",
                "16120389982",
                "16121632891",
                "16121635444",
                "16121641951",
                "16121653810",
                "16121718069",
                "16121720674",
                "16121724765",
                "16121797844",
                "16121928389",
                "16124004522",
                "16124353857",
                "16124377441",
                "16124383639",
                "16124395843",
                "16124400793",
                "16124437529",
                "16124441141",
                "16124454640",
                "16124466088",
                "16124467702",
                "16124484568",
                "16124488736",
                "16124497952",
                "16124499766",
                "16127709486",
                "16127722259",
                "16127726475",
                "16127820919",
                "16129374406",
                "16129403569",
                "16129412376",
                "16129516265",
                "16129564784",
                "16129594840",
                "16129595183",
                "16129820580",
                "16129873499",
                "16130526635",
                "16130552404",
                "16130818390",
                "16132077383",
                "16132699416",
                "16132822810",
                "16133260701",
                "16133281397",
                "16133305292",
                "16133549575",
                "16133551767",
                "16133657772",
                "16133660883",
                "16133903210",
                "16137438978",
                "16137452388",
                "16137456226",
                "16139363874",
                "16139436620",
                "16142499257",
                "16142499258",
                "16142499260",
                "16143270037",
                "16143727965",
                "16143743239",
                "16145459526",
                "16146563122",
                "16146571307",
                "16146571308",
                "16147127204",
                "16147166602",
                "16147188859",
                "16147223292",
                "16147226741",
                "16147261704",
                "16147269182",
                "16147279268",
                "16147291954",
                "16147294601",
                "16147295709",
                "16147297778",
                "16147298053",
                "16149070997",
                "16150542230",
                "16150542231",
                "16150779381",
                "16151265982",
                "16152181222",
                "16152208043",
                "16152208044",
                "16153053857",
                "16153605052",
                "16157124497",
                "16157239319",
                "16157458992",
                "16157948969",
                "16158396792",
                "16162050123",
                "16162054976",
                "16162464170",
                "16162610910",
                "16163491304",
                "16163498428",
                "16163498430",
                "16163498431",
                "16163498434",
                "16163498435",
                "16163498436",
                "16163498439",
                "16167125554",
                "16167337113",
                "16168186010",
                "16168187294",
                "16168191257",
                "16170654494",
                "16170658654",
                "16170662461",
                "16171302027",
                "16171318084",
                "16171320514",
                "16171338865",
                "16171366737",
                "16171371882",
                "16171381006",
                "16171385267",
                "16171394050",
                "16171463740",
                "16171463741",
                "16171463742",
                "16171463743",
                "16171463744",
                "16171463745",
                "16171463746",
                "16171463747",
                "16171463748",
                "16171463749",
                "16171463750",
                "16171463751",
                "16171463752",
                "16171463753",
                "16171463754",
                "16172134904",
                "16172199414",
                "16172218709",
                "16173428295",
                "16176529377",
                "16176540654",
                "16177553848",
                "16177887777",
                "16180254481",
                "16180267024",
                "16181215076",
                "16181583203",
                "16184570115",
                "16186638749",
                "16189732736",
                "16190558720",
                "16191386441",
                "16191389289",
                "16191395219",
                "16191401502",
                "16191495259",
                "16191511006",
                "16191923530",
                "16191958902",
                "16191963055",
                "16191984356",
                "16191987364",
                "16191989017",
                "16194795429",
                "16195206980",
                "16197804348",
                "16198408048",
                "16201298744",
                "16204080162",
                "16204392431",
                "16204401315",
                "16204404781",
                "16204424032",
                "16204441723",
                "16204445400",
                "16205342898",
                "16205457537",
                "16209664370",
                "16211090885",
                "16212067123",
                "16213248316",
                "16213256489",
                "16217806914",
                "16217838299",
                "16218567847",
                "16218569222",
                "16219629909",
                "16220415986",
                "16220418505",
                "16221862040",
                "16228909330",
                "16228909331",
                "16230696234",
                "16230702974",
                "16230707788",
                "16240346284",
                "16241570426",
                "16241613273",
                "16246223905",
                "16246223907",
                "16246223908",
                "16246223910",
                "16246223911",
                "16246223913",
                "16246223914",
                "16251965579",
                "16254317207",
                "16254547553",
                "16256668082",
                "16258070051",
                "16258080667",
                "16258081022",
                "16258084608",
                "16258085613",
                "16258085888",
                "16258891865",
                "16259056805",
                "16259374548",
                "16259950934",
                "16259954023",
                "16260087050",
                "16260088095",
                "16260496567",
                "16260502397",
                "16260511555",
                "16260519290",
                "16260524570",
                "16260529980",
                "16260552927",
                "16260570187",
                "16260578757",
                "16260694986"
            };
                

                foreach (var produto in produtoIdsAndNameList)
                {
                    foreach (var id in ids)
                    {
                        if (id == produto.Key)
                        {
                            Console.WriteLine(id + " = " + produto.Key);
                            idsResults.Add(produto.Key, produto.Value);                          
                        }                        
                    }
                }               

                foreach (var idResult in idsResults)
                {
                    Console.WriteLine("Obtendo histórico do produto: " + idResult.Key);                   

                    List<RegistroProdutoEstoque> produtoHistorico = _blingProdutoFetcher.GetRegistroProdutoEstoques(idResult.Key);

                    foreach (var produto in produtoHistorico) 
                    {
                        await _produtoHistoricoRepository.Add(idResult.Key, idResult.Value, produto);
                    }
                }

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

            stopwatch.Stop();
            Console.WriteLine($"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms");

            // Converte milissegundos para minutos
            double minutos = stopwatch.ElapsedMilliseconds / 60000.0;

            // Exibe o resultado
            Console.WriteLine($"Tempo em minutos: {minutos} minutos");


        }
    }
}

