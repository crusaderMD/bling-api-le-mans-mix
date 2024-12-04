using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Infrastructure;
using BlingApiDailyConsult.Repository;
using Microsoft.Extensions.Configuration;

namespace BlingApiDailyConsult.Tests
{
    internal class ProdutoFetcherTest
    {
        private readonly BlingProdutoFetcher _produtoFetcher;
        private readonly ProdutoRepository _produtoRepository;

        public ProdutoFetcherTest(TokenManager tokenManager, IConfiguration configuration)
        {
            _produtoFetcher = new BlingProdutoFetcher(tokenManager);
            _produtoRepository = new ProdutoRepository(configuration);
        }

        public async Task TestReqInsertProduto()
        {
            // Armazena os produtos
            Produto[] produtos = await _produtoFetcher.ExecuteAsync();            
            
            // Salva os produtos no BD
            _produtoRepository.Add(produtos);
        }
    }
}
