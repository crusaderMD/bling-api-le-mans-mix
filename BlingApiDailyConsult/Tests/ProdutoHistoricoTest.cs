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
        private readonly BlingProdutoFetcher _blingProdutoFetcher;
        private readonly ProdutoHistoricoRepository _produtoHistoricoRepository;

        public ProdutoHistoricoTest(IConfiguration configuration)
        {
            _produtoRepository = new ProdutoRepository(configuration);
            _blingProdutoFetcher = new BlingProdutoFetcher();
            _produtoHistoricoRepository = new ProdutoHistoricoRepository(configuration);
        }
        public async Task TestReqInsertHistoricoProduto()
        {
            Dictionary<string, string> produtoIdsAndNameList = await _produtoRepository.GetIdsAndNamesAsync();
            IEnumerable<RegistroProdutoEstoque> produtoHistorico = new List<RegistroProdutoEstoque>();

            foreach (var id in produtoIdsAndNameList)
            {
                Console.WriteLine("Obtendo o histórico do Produto: " + id);

                produtoHistorico = _blingProdutoFetcher.GetRegistroProdutoEstoques(id.Key);

                Console.WriteLine("Total elements: " + produtoHistorico.LongCount());

                foreach (var registroLine in produtoHistorico)
                {
                    await _produtoHistoricoRepository.Add(id.Key, id.Value, registroLine);
                }
            }
        }
    }
}
