using BlingApiDailyConsult.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace BlingApiDailyConsult.Infrastructure
{
    public class DataBaseHelper
    {
        private readonly string _connectionString;

        public DataBaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }



        /// <summary>
        /// Insere ou atualiza um token no banco de dados.
        /// </summary>

        public void InsertOrUpdateToken(TokenInfo tokenInfo)
        {
            if (tokenInfo == null)
            {
                throw new ArgumentNullException(nameof(tokenInfo), "O objeto TokenInfo não pode ser nulo.");
            }

            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = @"
                    INSERT INTO tokens
                        (access_token, expires_in, token_type, scope, refresh_token, created_at)
                    VALUES
                        (@access_token, @expires_in, @token_type, @scope, @refresh_token, @created_at)
                    ON DUPLICATE KEY UPDATE
                        access_token = VALUES(access_token),                        
                        expires_in = VALUES(expires_in),
                        token_type = VALUES(token_type),
                        scope = VALUES(scope),
                        refresh_token = VALUES(refresh_token),
                        created_at = VALUES(created_at);";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@access_token", tokenInfo.AccessToken);
                        cmd.Parameters.AddWithValue("@expires_in", tokenInfo.ExpiresIn);
                        cmd.Parameters.AddWithValue("@token_type", tokenInfo.TokenType);
                        cmd.Parameters.AddWithValue("@scope", tokenInfo.Scope);
                        cmd.Parameters.AddWithValue("@refresh_token", tokenInfo.RefreshToken);
                        cmd.Parameters.AddWithValue("@created_at", tokenInfo.DatetimeNowUtc);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"SQL Insert or Update ERROR: + {ex.Message}");
            }
        }
    }
}
