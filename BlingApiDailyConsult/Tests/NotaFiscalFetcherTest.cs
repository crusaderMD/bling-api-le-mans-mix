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
    internal class NotaFiscalFetcherTest
    {
        private readonly BlingNotaFiscalFetcher _blingNotaFiscalFetcher;
        private readonly BlingSingleNotaFiscalFetcher _blingSingleNotaFiscalFetcher;
        private readonly NotaFiscalRepository _notaFiscalRepository;


        public NotaFiscalFetcherTest(TokenManager tokenManager, IConfiguration configuration)
        {
            _blingNotaFiscalFetcher = new BlingNotaFiscalFetcher(tokenManager);
            _blingSingleNotaFiscalFetcher = new BlingSingleNotaFiscalFetcher(tokenManager);
            _notaFiscalRepository = new NotaFiscalRepository(configuration);
        }

        public async Task TestReqInsertNotaFiscal()
        {
            NotaFiscal[] notasFiscais = await _blingNotaFiscalFetcher.ExecuteAsync();
            List<NotaFiscal> listNfe = new List<NotaFiscal>();

            foreach (var notaFiscal in notasFiscais)
            {
                Console.WriteLine();
                Console.WriteLine($"Nota Fiscal número: { notaFiscal.Numero }, Id: {notaFiscal.Id}, Tipo: {notaFiscal.TipoNotaFiscal}, Situação: {notaFiscal.SituacaoNotaFiscal}, Valor:{notaFiscal.ValorNota}");

                var singleNotaFiscal = await _blingSingleNotaFiscalFetcher.GetSingleNotaFiscal(notaFiscal.Id);
                listNfe.Add(singleNotaFiscal);                

                await Task.Delay(500); // realiza 2 requisições por segundo

                Console.WriteLine();
                Console.WriteLine($"Nota Fiscal número: {singleNotaFiscal.Numero}, Id: {singleNotaFiscal.Id}, Tipo: {singleNotaFiscal.TipoNotaFiscal}, Situação: {singleNotaFiscal.SituacaoNotaFiscal}, Valor:{singleNotaFiscal.ValorNota}");
            }
            _notaFiscalRepository.Add(listNfe);
        }
    }
}
