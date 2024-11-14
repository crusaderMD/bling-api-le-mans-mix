using System;

namespace BlingApiDailyConsult.Infrastructure
{
    public class TokenHelper
    {
        // Método para verificar se o tempo do Token expirou
        public static bool IsTokenExpired(DateTime createdAt, int expiresIn)
        {
            // Verifica se a data de expiração já passou
            return DateTime.UtcNow >= createdAt.AddSeconds(expiresIn);
        }
    }
}
