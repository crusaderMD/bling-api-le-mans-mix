using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Repository
{
    internal class ProdutoHistoricoRepository : IRepository<IEnumerable<RegistroProdutoEstoque>>
    {
        private readonly string? _connectionString;

        public ProdutoHistoricoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Add(IEnumerable<RegistroProdutoEstoque> produtoEstoque)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<RegistroProdutoEstoque> entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetAllIdsAsync()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RegistroProdutoEstoque> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<RegistroProdutoEstoque> entity)
        {
            throw new NotImplementedException();
        }
    }
}
