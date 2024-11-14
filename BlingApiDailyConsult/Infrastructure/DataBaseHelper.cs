using BlingApiDailyConsult.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Mysqlx;

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

        public TokenInfo GetTokenFromDatabase()
        {
            TokenInfo tokenInfo = null;
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = @"
                        SELECT Access_Token, Expires_In, Token_Type, Scope, Refresh_Token, Created_at
                        FROM tokens
                        WHERE created_at = (SELECT MAX(created_at) FROM tokens);";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tokenInfo = new TokenInfo
                                {
                                    AccessToken = reader.GetString(reader.GetOrdinal("Access_Token")),
                                    ExpiresIn = reader.GetInt32(reader.GetOrdinal("Expires_In")),
                                    TokenType = reader.GetString(reader.GetOrdinal("Token_Type")),
                                    Scope = reader.GetString(reader.GetOrdinal("Scope")),
                                    RefreshToken = reader.GetString(reader.GetOrdinal("Refresh_Token")),
                                    DatetimeNowUtc = reader.GetDateTime(reader.GetOrdinal("Created_at"))
                                };
                            }
                            if (tokenInfo == null)
                            {
                                throw new InvalidOperationException("Nenhum token foi encontrado na base de dados. Por favor, insira um token inicial usando o fluxo de autenticação.");
                            }
                        }
                    }
                }
                
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Erro ao buscar token do banco de dados: {ex.Message}", ex);
            }

            return tokenInfo;
        }
    }
}
