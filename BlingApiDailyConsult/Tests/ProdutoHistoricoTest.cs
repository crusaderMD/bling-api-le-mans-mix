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
    internal class ProdutoHistoricoTest
    {
        private readonly ProdutoRepository _produtoRepository;
        private readonly BlingProdutoHistoricoFetcher _blingProdutoHistoricoFetcher;
        private readonly ProdutoHistoricoRepository _produtoHistoricoRepository;

        int produtoCount = 0;

        public ProdutoHistoricoTest(IConfiguration configuration)
        {
            _produtoRepository = new ProdutoRepository(configuration);
            _blingProdutoHistoricoFetcher = new BlingProdutoHistoricoFetcher();
            _produtoHistoricoRepository = new ProdutoHistoricoRepository(configuration);
        }
        public async Task TestReqInsertHistoricoProduto()
        {
            Dictionary<string, string> produtoIdsAndNameList = await _produtoRepository.GetIdsAndNamesAsync();
            IEnumerable<RegistroProdutoEstoque> produtoHistorico = new List<RegistroProdutoEstoque>();

            foreach (var id in produtoIdsAndNameList)
            {
                Console.WriteLine();
                Console.WriteLine("Obtendo o histórico do Produto: " + id);

                produtoHistorico = _blingProdutoHistoricoFetcher.GetRegistroProdutoEstoque(id.Key);

                Console.WriteLine("Total elements: " + produtoHistorico.LongCount());

                foreach (var registroLine in produtoHistorico)
                {
                    await _produtoHistoricoRepository.Add(id.Key, id.Value, registroLine);
                }

                produtoCount++;
            }

            List<string> idErrorList = _blingProdutoHistoricoFetcher.getIdErrorList();

            foreach (string id in idErrorList)
            {
                Console.WriteLine(id);
            }

            _blingProdutoHistoricoFetcher.Dispose();

            Console.WriteLine(produtoCount);
        }
    }
}
