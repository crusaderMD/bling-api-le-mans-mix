using System;

namespace BlingApiDailyConsult.Infrastructure
{
    public class TokenHelper
    {
        /// <summary>
        /// Verifica se o token de acesso está expirado.
        /// </summary>
        /// <param name="createdAt">A data/hora em que o token foi criado.</param>
        /// <param name="expiresIn">O tempo de expiração do token em segundos.</param>
        /// <returns>True se o token estiver expirado; caso contrário, False.</returns>
        public static bool IsTokenExpired(DateTime createdAt, int expiresIn)
        {
            // Testa se o expiresIn não é negativo
            if (expiresIn <= 0)
            {
                throw new ArgumentException("O tempo de expiração deve ser positivo.", nameof(expiresIn));
            }
            return DateTime.UtcNow >= createdAt.AddSeconds(expiresIn);
        }

        /// <summary>
        /// Verifica se o refresh token está expirado.
        /// </summary>
        /// <param name="createdAt">A data/hora em que o refresh token foi criado.</param>
        /// <param name="validityDays">O tempo de expiração do refresh token em dias.</param>
        /// <returns>True se o refresh token estiver expirado; caso contrário, False.</returns>
        public static bool IsRefreshTokenExpired(DateTime createdAt, int validityDays)
        {
            // Testa se o validityDays não é negativo
            if (validityDays <= 0)
            {
                throw new ArgumentException("O tempo de expiração deve ser positivo.", nameof(validityDays));
            }
            return DateTime.UtcNow >= createdAt.AddDays(validityDays);
        }
    }
}
