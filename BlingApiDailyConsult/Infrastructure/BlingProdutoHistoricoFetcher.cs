using BlingApiDailyConsult.Entities;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Infrastructure
{
    internal class BlingProdutoHistoricoFetcher
    {
        private IWebDriver? _driver = null; // Mantém o WebDriver aberto entre chamadas

        private List<Cookie>? storedCookies = null; // Armazena cookies após o login

        // Alterar na migração para servidor
        private readonly string driverPath = @"C:\Users\lemans\Downloads\chromedriver-win64";

        private readonly string loginUrl = "https://www.bling.com.br/login";

        // Alterar para variaveis de ambiente na migração para servidor
        private readonly string username = "lemansmix";
        private readonly string password = "Compras-159luiz";

        private List<string> idErrorList = new List<string>();

        public List<RegistroProdutoEstoque> GetRegistroProdutoEstoque(string produtoId)
        {
            // Lista de registros que será retornada
            List<RegistroProdutoEstoque> registros = new List<RegistroProdutoEstoque>();

            try
            {
                if (string.IsNullOrWhiteSpace(produtoId))
                {
                    Console.WriteLine("O produtoId é nulo ou vazio. Ignorando esta entrada.");
                    return registros;
                }

                // Inicializa o driver apenas uma vez
                if (_driver == null)
                {
                    _driver = InitializeDriver();
                    PerformLogin();
                }
                else
                {
                    // Reutiliza cookies para evitar relogar
                    if (storedCookies != null)
                    {
                        _driver.Manage().Cookies.DeleteAllCookies();

                        // Apagar depois
                        Console.WriteLine("Cookies capturados:");

                        foreach (Cookie cookie in storedCookies)
                        {
                            _driver.Manage().Cookies.AddCookie(cookie);

                            // Apagar depois
                            Console.WriteLine($"Nome: {cookie.Name}, Valor: {cookie.Value}");
                        }
                    }
                    //Thread.Sleep(5000);                    
                    _driver.Navigate().Refresh();
                }

                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                if (_driver.Url.Contains("bling.com.br/b/inicio") || _driver.Url.Contains("bling.com.br/estoque.php?buscaid="))
                {
                    // Acessa a URL de consulta do histórico do produto
                    var targetUrl = $"https://www.bling.com.br/estoque.php?buscaid={produtoId}";

                    // Apagar depois
                    Console.WriteLine($"URL atual antes da navegação: {_driver?.Url}");

                    _driver?.Navigate().GoToUrl(targetUrl);

                    // Apagar depois
                    Console.WriteLine($"URL atual após a navegação: {_driver?.Url}");

                    wait.Until(d => d.Url.Contains("estoque.php")); // Aguarda redirecionamento

                    // Aguarda o carregamento completo da página
                    wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").ToString() == "complete");

                    registros = ParseHistoricoPage();

                    Console.WriteLine($"Consulta concluída para Produto ID: {produtoId}");
                }
                else
                {
                    Console.WriteLine("Erro ao carregar a página de busca");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar o produto ID {produtoId}: {ex.Message}");
                addIdErrorList(produtoId); // Armazena o produto com erro
            }
            return registros;
        }

        private IWebDriver? InitializeDriver()
        {
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
            chromeOptions.AddArgument("--ignore-certificate-errors");
            chromeOptions.AddArgument("--window-size=1920,1080"); // Configurar o tamanho da janela virtual           

            // Inicializa o ChromeDriver com as opçoes
            return new ChromeDriver(driverPath, chromeOptions); // se retirar o chromeOptions, abre o navegador
        }

        private void PerformLogin()
        {
            Console.WriteLine("Iniciando login...");
            _driver?.Navigate().GoToUrl(loginUrl);



            // Uso de WebDriverWait para aguardar os elementos de login
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            try
            {
                // Encontrar os campos de login e preencher com os dados
                var emailField = wait.Until(d => d.FindElement(By.Id("username"))); // Seletor de Id para campo de username

                emailField.SendKeys(username);

                var passwordField = wait.Until(d => d.FindElement(By.CssSelector("input[type='password'].InputText-input"))); // Seletor de classe para o campo de senha

                passwordField?.SendKeys(password);

                // Encontrar o botão de login e clicar
                var loginButton = wait.Until(d => d.FindElement(By.CssSelector("button[type='submit']")));
                loginButton?.Click();
                SimulateUserInteraction();

                // Captura os cookies após login bem-sucedido
                storedCookies = _driver?.Manage().Cookies.AllCookies.ToList();

                Console.WriteLine("Login bem-sucedido e cookies armazenados.");

                foreach (Cookie cookie in storedCookies)
                {
                    // Apagar depois
                    Console.WriteLine($"Nome: {cookie.Name}, Valor: {cookie.Value}");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Falha no login. Verifique as credenciais ou a página.");
                throw;
            }
        }

        private List<RegistroProdutoEstoque> ParseHistoricoPage()
        {
            // Lista de registros que será retornada
            List<RegistroProdutoEstoque> registros = new List<RegistroProdutoEstoque>();

            bool hasNextPage = true;

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)); // Espera máxima de 10 segundos

            while (hasNextPage)
            {
                try
                {
                    // Esperar até que o conteúdo da tabela 'datatable' seja carregado na página
                    //wait.Until(d => d.FindElement(By.Id("datatable")).Displayed);

                    // Esperar um pouco para a página carregar
                    Thread.Sleep(5000);


                    // Pegar o HTML da página de consulta
                    string pageSource = _driver.PageSource;

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

                                // Método auxiliar para verificar o valor
                                decimal ParseOrDefault(string value, decimal defaultValue = 0)
                                {
                                    return value != "-" ? decimal.Parse(value) : defaultValue;
                                }

                                // Mapeia os dados extraídos para a entidade
                                var registro = new RegistroProdutoEstoque
                                {
                                    Data = DateTime.Parse(data),
                                    Entrada = entrada,
                                    Saida = saida,
                                    PrecoVenda = ParseOrDefault(precoVenda),
                                    PrecoCompra = ParseOrDefault(precoCompra),
                                    PrecoCusto = ParseOrDefault(precoCusto),
                                    Observacao = observacao,
                                    Origem = origem,
                                    Tipo = tipo
                                };

                                // Adiciona o registro à lista
                                registros.Add(registro);
                            }

                        }
                        else
                        {
                            Console.WriteLine($"Div 'datatable' não encontrada");
                        }
                        try
                        {
                            // Localizar o botão "Próxima"
                            var botaoProxima = _driver.FindElement(By.XPath("//li[not(contains(@class, 'disabled'))]//span[contains(text(),'Próxima')]"));

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
                    Console.WriteLine($"Erro ao processar produtoId: {ex.Message}");
                }
            }
            // Retornar a lista completa de registros após processar todas as páginas
            return registros;
        }
        private void addIdErrorList(string id)
        {
            idErrorList.Add(id);
        }

        public List<string> getIdErrorList()
        {
            return idErrorList;
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver = null;
            Console.WriteLine("Driver encerrado com sucesso.");
        }

        private void SimulateUserInteraction()
        {
            Random rand = new Random();
            // Pause aleatória entre 1 a 3 segundos
            int delay = rand.Next(1000, 10000);
            Console.WriteLine($"Esperando {delay / 1000} segundos para simular interação humana...");
            Thread.Sleep(delay);
        }
    }
}