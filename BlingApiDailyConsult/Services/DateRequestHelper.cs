namespace BlingApiDailyConsult.Services
{
    public class DateRequestHelper
    {
        private readonly DateTime today;
        private readonly DateTime startDate;

        public DateRequestHelper()
        {
            today = DateTime.Now; // Data atual
            startDate = new DateTime(today.Year, 1, 1); // Primeiro dia do ano atual
        }

        // Método que retorna o intervalo de datas formatado para adicionar a URL
        public string GetDateQueryString()
        {
            return $"&dataInicial={startDate:yyyy-MM-dd}&dataFinal={today:yyyy-MM-dd}";
        }
    }
}

