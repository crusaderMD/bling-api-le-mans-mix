using System.Security.Cryptography;

namespace BlingApiDailyConsult.Services
{
    public class DateRequestHelper
    {
        private readonly DateTime today;
        private  DateTime startDate;
        private DateTime endDate;

        public DateRequestHelper(int? year = null, int? month = null)
        {
            today = DateTime.Now; // Data atual
            
            // Define o ano e mês, com valores padrão para o mês atual
            int targetYear = year ?? today.Year;
            int tagetMonth = month ?? today.Month;

            // Configura a data de início e fim do mês
            startDate = new DateTime(targetYear, tagetMonth, 1); // Primeiro dia do mês
            endDate = startDate.AddMonths(1).AddDays(-1); // Último dia do mês
        }

        // Método que retorna o intervalo de datas formatado para adicionar a URL
        public string GetDateQueryString()
        {
            return $"&dataInicial={startDate:yyyy-MM-dd}&dataFinal={endDate:yyyy-MM-dd}";
        }
    }
}

