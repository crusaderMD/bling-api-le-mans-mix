using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Infrastructure;
using BlingApiDailyConsult.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Tests
{     
    internal class PedidoCompraFetcherTest
    {
        private readonly BlingPedidoCompraFetcher _blingPedidoCompraFetcher;
        private readonly PedidoCompraRepository _pedidoCompraRepository;

        public PedidoCompraFetcherTest(TokenManager tokenManager, IConfiguration configuration)
        {
            _blingPedidoCompraFetcher = new BlingPedidoCompraFetcher(tokenManager);
            _pedidoCompraRepository = new PedidoCompraRepository(configuration);
        }

        public async Task TestReqInsertPedidoCompra()
        {
            Pedido[] pedidosCompra = await _blingPedidoCompraFetcher.ExecuteAsync();

            _pedidoCompraRepository.Add(pedidosCompra);
        }
    }
}
