using Microsoft.AspNetCore.Mvc;
using BlingApiDailyConsult.Infrastructure;
using System.Threading.Tasks;
using System.Text.Json;
using BlingApiDailyConsult.Entities;
using Microsoft.Extensions.Logging;

namespace BlingOAuthApi.Controllers
{
    [ApiController]
    [Route("callback")] // Define a rota como exatamente /callback
    public class CallbackController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CallbackController> _logger;

        public CallbackController(IConfiguration configuration, ILogger<CallbackController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Captura o c�digo de autoriza��o enviado pelo Bling e obt�m o token de acesso
        /// </summary>
        /// <param name="code">C�digo de autoriza��o enviado como par�metro na URL</param>
        /// <param name="state">Estado (state) enviado como par�metro na URL</param>
        /// <returns>Mensagem de sucesso ou erro</returns>
        [HttpGet]
        public async Task<IActionResult> GetAuthorizationCode([FromQuery] string code, [FromQuery] string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Authorization code n�o encontrado na URL.");
            }

            if (string.IsNullOrEmpty(state))
            {
                return BadRequest("State n�o encontrado ou inv�lido.");
            }

            // Log para depura��o
            _logger.LogInformation($"Authorization Code: {code}");
            _logger.LogInformation($"State: {state}");

            try
            {

                // Chama o m�todo da classe OAuthHelperGetTokens para obter o token
                var tokenResponse = await OAuthHelperGetTokens.GetAccessTokenAsync(code);

                tokenResponse = tokenResponse.Trim();

                if (string.IsNullOrEmpty(tokenResponse))
                {
                    _logger.LogError("Token Response est� vazio ou nulo.");
                    return BadRequest("Token Response est� vazio ou nulo.");
                }

                _logger.LogInformation($"Token Response: {tokenResponse}");                

                // Deserializar o JSON de resposta para um objeto TokenInfo
                var tokenInfo = JsonSerializer.Deserialize<TokenInfo>(tokenResponse);
                _logger.LogInformation("Token deserializado com sucesso.");                                             

                if (tokenInfo == null)
                {
                    //return BadRequest("TOKEN NULO: N�o foi poss�vel deserializar as informa��es do token.");
                }
                
                // Valida��es b�sicas no token
                if (string.IsNullOrEmpty(tokenInfo.AccessToken) || string.IsNullOrEmpty(tokenInfo.RefreshToken))
                {
                    return BadRequest(new { Message = "Token de acesso ou refresh token est� vazio." });
                }              

                // Grava no banco de dados usando DatabaseHelper
                var dbHelper = new DataBaseHelper(_configuration);
                dbHelper.InsertOrUpdateToken(tokenInfo);

                // Retorna uma resposta de sucesso
                return Ok(new
                {
                    Message = "Authorization code capturado e token de acesso obtido e salvo com sucesso!",
                    Code = code,
                    State = state,
                    TokenResponse = tokenResponse,
                    TokenInfo = tokenInfo // Remova isso em produ��o
                });
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Erro ao processar o token.");
                return StatusCode(500, $"Erro ao processar o token: {ex.Message}");
            }
        }
    }
}
