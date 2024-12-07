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
    internal class PedidoVendaFetcherTest
    {
        private readonly BlingPedidoFetcher _blingPedidoFetcher;
        private readonly PedidoVendaRepository _pedidoVendaRepository;

        public PedidoVendaFetcherTest(TokenManager tokenManager, IConfiguration configuration)
        {
            _blingPedidoFetcher = new BlingPedidoFetcher(tokenManager);
            _pedidoVendaRepository = new PedidoVendaRepository(configuration);
        }

        public async Task TesteReqInsertPedidoVenda()
        {

            // Armazena os pedidos request da API
            Pedido[] pedidos = await _blingPedidoFetcher.ExecuteAsync();

            // Obtem os Ids dos pedidos na tabela pedidos no BD
            List<string> pedidosIds = (List<string>)await _pedidoVendaRepository.GetAllIdsAsync();

            // Filtra os pedidos já inseridos no BD
            pedidos = pedidos.Where(pedido => !pedidosIds.Contains(pedido.Id.ToString())).ToArray();

            // Salva os pedidos no BD
            _pedidoVendaRepository.Add(pedidos);
        }
    }
}
