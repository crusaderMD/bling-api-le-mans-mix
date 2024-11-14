using System.Text.Json;
using System.Threading.Tasks;
using BlingApiDailyConsult.Entities;

namespace BlingApiDailyConsult.Infrastructure
{
    public class TokenManager
    {
        private readonly DataBaseHelper _dataBaseHelper;
        private const int RefreshTokenValidityDays = 30;  // 30 dias para a validade do refresh token

        public TokenManager(DataBaseHelper dataBaseHelper)
        {
            _dataBaseHelper = dataBaseHelper ?? throw new ArgumentNullException(nameof(dataBaseHelper));
        }

        // Método para verificar se o token está expirado e, se necessário, fazer o refresh
        public async Task<string> GetValidAccessTokenAsync()
        {
            // Recupera o Token atual do banco de dados
            TokenInfo tokenInfo = _dataBaseHelper.GetTokenFromDatabase();

            // Verifica se o token existe no banco de dados
            if (tokenInfo == null)
            {
                throw new ArgumentNullException(nameof(tokenInfo), "O banco de dados retornou um token nulo.");
            }

            // Verifica se o AccessToken expirou
            if (TokenHelper.IsTokenExpired(tokenInfo.DatetimeNowUtc, tokenInfo.ExpiresIn))
            {
                // Se o Refresh_Token também tiver expirado, reinicia o processo de autenticação
                if (TokenHelper.IsTokenExpired(tokenInfo.DatetimeNowUtc, RefreshTokenValidityDays))
                {
                    // O refresh token expirou, logo, inicia o processo de autenticação
                    OAuthHelperGetAuthCode.RedirectToAuthUrl(); // Redireciona o usuário para obter o código de autorização

                    throw new Exception("Ambos os tokens expiraram, o processo de autenticação precisa ser reiniciado.");
                }

                // O access_token expirou, mas o refresh_token ainda é válido
                string refreshAccessToken = await OAuthHelperGetTokens.RefreshAccessTokenAsync(tokenInfo.RefreshToken);

                TokenInfo newTokenInfo = JsonSerializer.Deserialize<TokenInfo>(refreshAccessToken);

                if (newTokenInfo == null || string.IsNullOrEmpty(newTokenInfo.AccessToken) || string.IsNullOrEmpty(newTokenInfo.RefreshToken))
                {
                    throw new Exception("Falha ao deserializar o novo token ou dados incompletos.");
                }

                // Atualizar o banco de dados com o novo token
                _dataBaseHelper.InsertOrUpdateToken(newTokenInfo);

                return newTokenInfo.AccessToken;
            }
            return tokenInfo.AccessToken;
        }
    }
}
