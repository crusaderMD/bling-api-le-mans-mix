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

            //Instanciar o BlingDataFetcher injetando o TokenManager
            var blingPedidoFetcher = new BlingPedidoFetcher(tokenManager);

            try
            {
                //Armazena os pedidos
                Pedido[] pedidos = await blingPedidoFetcher.ExecuteAsync();

                //Salva os pedidos no BD
                dataBaseHelper.SavePedidos(pedidos);

                // Confirmando que os dados foram armazenados
                Console.WriteLine("Dados obtidos e armazenados no banco com sucesso!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }
    }
}

