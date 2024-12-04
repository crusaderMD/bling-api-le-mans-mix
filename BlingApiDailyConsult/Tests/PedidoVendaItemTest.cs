using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Infrastructure;
using BlingApiDailyConsult.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Tests
{
    internal class PedidoVendaItemTest
    {
        private readonly BlingPedidoItemFetcher _blingPedidoItemFetcher;
        private readonly PedidoVendaRepository _pedidoVendaRepository;
        private readonly PedidoVendaItemRepository _pedidoItemRepository;

        public PedidoVendaItemTest(TokenManager tokenManager, IConfiguration configuration)
        {
            _blingPedidoItemFetcher = new BlingPedidoItemFetcher(tokenManager);
            _pedidoVendaRepository = new PedidoVendaRepository(configuration);
            _pedidoItemRepository = new PedidoVendaItemRepository(configuration, tokenManager);
        }
        public async Task TestReqInsertVendaItem()
        {

            List<string> pedidoIds = (List<string>)await _pedidoVendaRepository.GetAllIdsAsync();

            foreach (var id in pedidoIds)
            {
                Console.WriteLine(id);
            }

            Dictionary<string, List<Item>> pedidoProdutos = await _blingPedidoItemFetcher.FetchItensDosPedidosAsync(pedidoIds);

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

            // Salva os itens do pedido no BD
            _pedidoItemRepository.Add(pedidoProdutos);
        }
    }
}
