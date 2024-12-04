using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Infrastructure
{
    internal class BlingPedidoItemFetcher : IBlingApiFetcher<Pedido>
    {
        private readonly string _baseUrl = "https://api.bling.com.br/Api/v3/pedidos/vendas/";
        private readonly HttpClientRequestHelper _httpClientRequestHelper;

        public BlingPedidoItemFetcher(TokenManager tokenManager)
        {
            _httpClientRequestHelper = new HttpClientRequestHelper(tokenManager) ?? throw new ArgumentNullException(nameof(HttpClientRequestHelper));
        }

        public Task<Pedido> ExecuteAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, List<Item>>> FetchItensDosPedidosAsync(List<string> pedidoIds)
        {
            if (pedidoIds == null || !pedidoIds.Any())
            {
                throw new ArgumentException($"A lista de IDs de pedidos não pode ser nula ou vazia. {nameof(BlingPedidoItemFetcher)}, {nameof(FetchItensDosPedidosAsync)}, {nameof(pedidoIds)}");
            }

            var pedidosItensMap = new Dictionary<string, List<Item>>();
            var throttle = new SemaphoreSlim(3); // Limita para 3 requisições simultâneas
            var tasks = pedidoIds.Select(async pedidoId =>
            {
                await throttle.WaitAsync();
                try
                {
                    await Task.Delay(500);
                    string url = $"{_baseUrl}{pedidoId}";
                    var apiResponse = await _httpClientRequestHelper.FetchDataAsync<ApiSingleResponse<Pedido>>(url);

                    if (apiResponse?.Data != null)
                    {
                        var itens = apiResponse.Data.Itens ?? Enumerable.Empty<Item>();                           
                        lock (pedidosItensMap)
                        {
                            pedidosItensMap[pedidoId] = itens.ToList();
                        }
                        await Task.Delay(500);
                    }
                    else
                    {
                        Console.WriteLine($"Nenhum item encontrado para o pedido {pedidoId}.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar o pedido {pedidoId}: {ex.Message}");
                }
                finally
                {
                    throttle.Release();
                }
            });

            await Task.WhenAll(tasks);
            return pedidosItensMap;
        }
    }
}


    

