using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Infrastructure;
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
            //var blingPedidoFetcher = new BlingPedidoFetcher(tokenManager);

            // Instanciar o BlingProdutoFetcher
            //var blingProdutoFetcher = new BlingProdutoFetcher(tokenManager);

            var blingPedidoItemFetcher = new BlingPedidoItemFetcher(tokenManager);

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

                /*List<string> pedidoIds = dataBaseHelper.GetPedidoIdFromDataBase();

                foreach (var id in pedidoIds)
                {
                    Console.WriteLine(id);
                }*/

                List<string> pedidoIds = new() 
                {
                    "17991710971",
                    "18012150210",
                    "18012404988"
                };

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

                dataBaseHelper.SavePedidoItens(pedidoProdutos);

                foreach (var pedidoId in pedidoProdutos)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Pedido: {pedidoId.Key}");
                    foreach (var item in pedidoId.Value)
                    {
                        Console.WriteLine($"Produto: {item?.Produto?.Id}, Descrição: {item?.Descricao}, Quantidade: {item?.Quantidade}, Preço: {item?.Valor}");
                    }                    
                }

                /*using var client = new HttpClient();

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + "7090edd2463c6bdf8bc4da2f160be303c94272a9");

                HttpResponseMessage response = await client.GetAsync("https://api.bling.com.br/Api/v3/produtos?15949857484");

                Console.WriteLine(response);*/
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }
    }
}

