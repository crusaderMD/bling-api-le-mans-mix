using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
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

                foreach (var pedidoId in pedidoProdutos)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Pedido: {pedidoId.Key}");
                    foreach (var item in pedidoId.Value)
                    {
                        Console.WriteLine($"Produto: {item?.Produto?.Id}, Quantidade: {item?.Quantidade}, Preço: {item?.Valor}");
                    }                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }
    }
}

