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
    internal class PedidoCompraItemTest
    {
        private readonly BlingPedidoCompraItemFetcher _blingPedidoCompraItemFetcher;
        private readonly PedidoCompraRepository _pedidoCompraRepository;
        private readonly PedidoCompraItemRepository _pedidoCompraItemRepository;

        public PedidoCompraItemTest(TokenManager tokenManager, IConfiguration configuration)
        {
            _blingPedidoCompraItemFetcher = new BlingPedidoCompraItemFetcher(tokenManager);
            _pedidoCompraRepository = new PedidoCompraRepository(configuration);
            _pedidoCompraItemRepository = new PedidoCompraItemRepository(configuration, tokenManager);
        }

        public async Task TestReqInsertItemPedidoCompra()
        {
            List<string> pedidoCompraIds = (List<string>)await _pedidoCompraRepository.GetAllIdsAsync();

            foreach (var ids in pedidoCompraIds)
            {
                Console.WriteLine(ids);
            }

            Dictionary<string, List<Item>> pedidoCompraItens = await _blingPedidoCompraItemFetcher.FetchItensDosPedidosAsync(pedidoCompraIds);

            foreach (var pedido in pedidoCompraItens)
            {
                Console.WriteLine();
                Console.WriteLine("Pedido: " + pedido.Key);
                Console.WriteLine();

                foreach (var item in pedido.Value)
                {
                    Console.WriteLine("Item do pedido: " + item?.Produto?.Id);
                }
            }

            // Salva os itens dos pedidos no BD
            _pedidoCompraItemRepository.Add(pedidoCompraItens);
        }
    }
}
