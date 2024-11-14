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

            // Criando a instância de BlingDataFetcher
            //var blingDataFetcher = new BlingDataFetcher(configuration);

            // Chamando o método que fará a requisição e salvará os dados no banco
            //await blingDataFetcher.FetchAndStoreData();

            // Confirmando que os dados foram armazenados
            // Console.WriteLine("Dados obtidos e armazenados no banco com sucesso!");

            //Testando a chamada para obter o authorization code
            //OAuthHelperGetAuthCode.RedirectToAuthUrl();
            
            TokenManager tokenManager = new TokenManager(dataBaseHelper);
            try
            {
                string validToken = await tokenManager.GetValidAccessTokenAsync();
                Console.WriteLine($"Valid Token: {validToken}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }

        }
    }
}

