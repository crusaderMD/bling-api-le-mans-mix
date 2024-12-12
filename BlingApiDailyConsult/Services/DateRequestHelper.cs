using System;

namespace BlingApiDailyConsult.Services
{
    public class DateRequestHelper
    {
        private readonly DateTime today;
        private readonly DateTime startDate;
        private readonly DateTime endDate;

        public DateRequestHelper()
        {
            today = DateTime.Now; // Data atual
            endDate = today; // A data final é hoje
            startDate = today.AddDays(-30); // A data inicial é 30 dias antes de hoje
        }

        public string GetStartDate()
        {
            return $"{startDate:yyyy-MM-dd}";
        }

        public string GetEndDate()
        {
            return $"{endDate:yyyy-MM-dd}";
        }

        // Método que retorna o intervalo de datas formatado para adicionar à URL
        public string GetDateQueryString()
        {
            return $"&dataInicial={startDate:yyyy-MM-dd}&dataFinal={endDate:yyyy-MM-dd}";
        }
    }
}
