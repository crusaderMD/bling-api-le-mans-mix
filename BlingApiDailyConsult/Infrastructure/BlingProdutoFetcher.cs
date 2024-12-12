using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Interfaces;
using BlingApiDailyConsult.Services;
using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace BlingApiDailyConsult.Infrastructure
{
    internal class BlingProdutoFetcher : IBlingApiFetcher<Produto[]>
    {
        // URL da API Bling com os parâmetros para consulta de produtos
        private const string baseUrl = "https://api.bling.com.br/Api/v3/produtos?";

        private readonly HttpClientRequestHelper _httpClientHelper;
        private readonly PaginationHelper _paginationHelper;

        public BlingProdutoFetcher(TokenManager tokenManager)
        {
            _httpClientHelper = new HttpClientRequestHelper(tokenManager);
            _paginationHelper = new PaginationHelper();
        }

        public async Task<Produto[]> ExecuteAsync()
        {
            return (await _paginationHelper.FetchAllPagesAsync<Produto>(baseUrl, async (paginatedUrl) =>
            {
                var apiProdutoResponse = await _httpClientHelper.FetchDataAsync<ApiResponse<Produto>>(paginatedUrl);

                return apiProdutoResponse?.Data?.ToList() ?? new List<Produto>();
            }

            )).ToArray();
        }

        public List<RegistroProdutoEstoque> GetRegistroProdutoEstoques(string produtoId)
        {
            // Lista de registros que será retornada
            List<RegistroProdutoEstoque> registros = new List<RegistroProdutoEstoque>();

            IWebDriver? driver = null;

            bool hasNextPage = true; // Controla o loop de Paginação

            try
            {
                if (string.IsNullOrWhiteSpace(produtoId))
                {
                    Console.WriteLine("O produtoId é nulo ou vazio. Ignorando esta entrada.");
                    // Retorna a lista vazia
                    return registros;
                }

                // Caminho para o driver do Chrome, alterar no futuro
                var driverPath = @"C:\Users\lemans\Downloads\chromedriver-win64"; // Altere para o local onde estará seu chromedriver no futuro

                if (!Directory.Exists(driverPath))
                {
                    throw new FileNotFoundException("O caminho para o ChromeDriver não existe ou está incorreto.", driverPath);
                }

                // Configurando opções para o ChromeDriver
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArgument("--headless"); // Executar em modo headless
                chromeOptions.AddArgument("--disable-gpu"); // Necessário para compatibilidade em alguns sistemas
                chromeOptions.AddArgument("--no-sandbox"); // Recomendado para sistemas Linux
                chromeOptions.AddArgument("--disable-dev-shm-usage"); // Evitar problemas de memória compartilhada em Docker ou Linux
                chromeOptions.AddArgument("--window-size=1920,1080"); // Configurar o tamanho da janela virtual


                // Inicializar o ChromeDriver com as opções
                driver = new ChromeDriver(driverPath, chromeOptions); // se retirar o chromeOptions, abre o navegador

                // URL de login
                var loginUrl = "https://www.bling.com.br/login";
                var targetUrl = "https://www.bling.com.br/estoque.php?buscaid=" + produtoId;

                // Acessar a página de login
                driver.Navigate().GoToUrl(loginUrl);

                // Encontrar os campos de login e preencher com os dados
                var emailField = driver.FindElement(By.Id("username")); // Seletor de Id para campo de username
                var passwordField = driver.FindElement(By.CssSelector("input[type='password'].InputText-input")); // Seletor de classe para o campo de senha

                emailField.SendKeys("lemansmix");
                passwordField.SendKeys("Compras-159luiz");

                // Encontrar o botão de login e clicar
                var loginButton = driver.FindElement(By.CssSelector("button[type='submit']"));
                loginButton.Click();

                // Aguardar um tempo para o login ser processado
                Thread.Sleep(5000); // Você pode aumentar o tempo se necessário para o login ser concluído

                // Capturar os cookies após o login
                var cookies = driver.Manage().Cookies.AllCookies;

                Console.WriteLine("Cookies capturados:");
                foreach (var cookie in cookies)
                {
                    driver.Manage().Cookies.AddCookie(cookie);
                    Console.WriteLine($"Nome: {cookie.Name}, Valor: {cookie.Value}");
                }

                // Agora acessar a página de consulta
                driver.Navigate().GoToUrl(targetUrl);

                while (hasNextPage)
                {
                    // Esperar um pouco para a página carregar
                    Thread.Sleep(5000);

                    // Pegar o HTML da página de consulta
                    string pageSource = driver.PageSource;

                    // Expressão regular para capturar o conteúdo até fechar o </div>
                    string pattern = @"<div id=""datatable""[^>]*>[\s\S]*?</div>";

                    Match match = Regex.Match(pageSource, pattern);

                    if (match.Success)
                    {
                        // Captura o conteúdo da div 'datatable'
                        string datatableContent = match.Value;
                        Console.WriteLine("Conteúdo capturado da div 'datatable':\n" + datatableContent);

                        // Agora usar HtmlAgilityPack para fazer o parsing do conteúdo da div capturada
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(datatableContent);

                        // Selecionando as linhas da tabela
                        var rows = doc.DocumentNode.SelectNodes("//table[@class='tabela-listagem']/tbody/tr");

                        if (rows != null)
                        {
                            foreach (var row in rows)
                            {
                                // Método auxiliar para capturar apenas o valor sem o rótulo
                                string GetCellValue(HtmlNode cell)
                                {
                                    // Tenta localizar o segundo span, que contém o valor relevante
                                    var valueNode = cell.SelectSingleNode("./span[2]") ?? cell.SelectSingleNode("./span[1]");
                                    return valueNode?.InnerText.Trim() ?? "-";
                                }

                                // Método auxiliar para capturar atributos dentro de uma célula
                                string GetAttributeValue(HtmlNode cell, string xpath, string attributeName)
                                {
                                    var node = cell.SelectSingleNode(xpath);
                                    return node?.GetAttributeValue(attributeName, "-") ?? "-";
                                }

                                // Captura os valores relevantes
                                var data = GetCellValue(row.SelectSingleNode(".//td[1]"));
                                var entrada = GetCellValue(row.SelectSingleNode(".//td[2]"));
                                var saida = GetCellValue(row.SelectSingleNode(".//td[3]"));
                                var precoVenda = GetCellValue(row.SelectSingleNode(".//td[4]"));
                                var precoCompra = GetCellValue(row.SelectSingleNode(".//td[5]"));
                                var precoCusto = GetCellValue(row.SelectSingleNode(".//td[6]"));
                                var observacao = GetCellValue(row.SelectSingleNode(".//td[7]"));
                                // Captura da origem
                                var origemNode = row.SelectSingleNode(".//td[8]");
                                var origem = GetCellValue(origemNode);

                                // Captura do atributo 'tipo' na origem
                                var tipo = GetAttributeValue(origemNode, ".//span[@tipo]", "tipo");

                                // Mapeia os dados extraídos para a entidade
                                var registro = new RegistroProdutoEstoque
                                {
                                    Data = data,
                                    Entrada = entrada,
                                    Saida = saida,
                                    PrecoVenda = precoVenda,
                                    PrecoCompra = precoCompra,
                                    PrecoCusto = precoCusto,
                                    Observacao = observacao,
                                    Origem = origem,
                                    Tipo = tipo
                                };

                                // Adiciona o registro à lista
                                registros.Add(registro);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Div 'datatable' não encontrada para o produtoId: {produtoId}.");
                    }
                    try
                    {
                        // Localizar o botão "Próxima"
                        var botaoProxima = driver.FindElement(By.XPath("//li[not(contains(@class, 'disabled'))]//span[contains(text(),'Próxima')]"));

                        if (botaoProxima != null && botaoProxima.Displayed)
                        {
                            botaoProxima.Click();
                            Console.WriteLine("Navegando para a próxima página.");
                        }
                        else
                        {
                            Console.WriteLine("Botão 'Próxima' não está disponível.");
                            hasNextPage = false;
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        Console.WriteLine("Erro ao localizar o botão 'Próxima': O elemento não foi encontrado.");
                        hasNextPage = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao navegar para a próxima página: {ex.Message}");
                        hasNextPage = false;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar produtoId '{produtoId}': {ex.Message}");
            }
            finally
            {
                // Garantir que o driver seja encerrado fechando o navegador
                driver?.Quit();
                driver?.Dispose(); // Adicionado para liberar recursos completamente
            }
            // Exibindo os registros
            foreach (var registro in registros)
            {
                Console.WriteLine($"Data: {registro.Data}, Entrada: {registro.Entrada}, " +
                    $"Saída: {registro.Saida}, Preço Venda: {registro.PrecoVenda}, " +
                    $"Preço Compra: {registro.PrecoCompra}, Preço Custo: {registro.PrecoCusto}, " +
                    $"Observação: {registro.Observacao}, Origem: {registro.Origem}, Tipo: {registro.Tipo}");
            }
            return registros;
        }
    }
}
