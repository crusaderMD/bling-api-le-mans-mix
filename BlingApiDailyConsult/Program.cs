using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Infrastructure;
using BlingApiDailyConsult.Repository;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
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

            // Instanciar o DataBaseHelper com a configuração carregada
            var dataBaseHelper = new DataBaseHelper(configuration);

            // Instanciar o TokenManager injetando o DataBaseHelper
            var tokenManager = new TokenManager(dataBaseHelper);

            // Instanciar o BlingPedidoFetcher injetando o TokenManager
            var blingPedidoFetcher = new BlingPedidoFetcher(tokenManager);

            // Instanciar o BlingProdutoFetcher
            var blingProdutoFetcher = new BlingProdutoFetcher(tokenManager);

            // Instanciar o BlingPedidoItemFetcher
            var blingPedidoItemFetcher = new BlingPedidoItemFetcher(tokenManager);

            // Instaciar o BlingPedidoCompraFetcher
            var blingPedidoCompraFetcher = new BlingPedidoCompraFetcher(tokenManager);

            try
            {
                // Armazena os pedidos
                //Pedido[] pedidos = await blingPedidoFetcher.ExecuteAsync();

                // Salva os pedidos no BD
                //dataBaseHelper.SavePedidos(pedidos);

                // Confirmando que os dados foram armazenados
                //Console.WriteLine("Dados dos pedidos obtidos e armazenados no banco com sucesso!");

                // Armazena os produtos
                //Produto[] produtos = await blingProdutoFetcher.ExecuteAsync();

                // Salva os produtos no BD
                //dataBaseHelper.SaveProdutos(produtos);

                // Confirmando que os dados foram armazenados
                //Console.WriteLine("Dados dos produtos obtidos e armazenados no banco com sucesso!");                          

                //string pedidos = "18049418711\r\n18241588960\r\n18304836400\r\n18312872544\r\n18361674704\r\n18373812949\r\n18375779487\r\n18378902730\r\n18378920948\r\n18382438365\r\n18385267679\r\n18385354703\r\n18385374226\r\n18385389792\r\n18388873923\r\n18391935156\r\n18392944333";

                //string[] sepPedidos = pedidos.Split("\r\n");

                //List<string> pedidoIds = new(sepPedidos);

                PedidoRepository pedidoRepository = new PedidoRepository(configuration);
                
                List<string> pedidoIds = (List<string>)await pedidoRepository.GetAllIdsAsync();

                foreach (var id in pedidoIds)
                {
                    Console.WriteLine(id);
                }

                Dictionary<string, List<Item>> pedidoProdutos = await blingPedidoItemFetcher.FetchItensDosPedidosAsync(pedidoIds);

                /*Dictionary<string, List<Item>> pedidoProdutos = new Dictionary<string, List<Item>>
                {
                    {
                        "123", new List<Item>
                        {
                            new Item
                            {
                                Id = 1,
                                Descricao = "produto_test",
                                Quantidade = 10,
                                Valor = 10.10M,
                                Produto = new Produto
                                {
                                    Id = 16349367187
                                }
                            }
                        }
                    }
                };*/

                foreach (var pedidoId in pedidoProdutos)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Program Class => Pedido: {pedidoId.Key}");
                    foreach (var item in pedidoId.Value)
                    {
                        Console.WriteLine($"Produto: {item?.Produto?.Id}, Descrição: {item?.Descricao}, Quantidade: {item?.Quantidade}, Preço: {item?.Valor}");
                    }
                    Console.WriteLine();
                }

                dataBaseHelper.SavePedidoItens(pedidoProdutos);
                

                /*using var client = new HttpClient();

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + "4ed764f71a6b236f70de88e07d17c78e0d7ede36");

                HttpResponseMessage response = await client.GetAsync("https://api.bling.com.br/Api/v3/produtos?16189732736");

                Console.WriteLine(response);

                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Exibe o JSON recebido para depuração
                Console.WriteLine();
                Console.WriteLine("JSON recebido:");
                Console.WriteLine(jsonResponse);
                Console.WriteLine();*/


            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro SQL: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro de Argumento: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro genérico: {ex.Message}");
            }

            Console.WriteLine("eu continuei!");

            try
            {
                Pedido[] pedidosCompra = await blingPedidoCompraFetcher.ExecuteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro genérico: {ex.Message}");
            }
        }
    }
}

