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
        /// Captura o código de autorização enviado pelo Bling e obtém o token de acesso
        /// </summary>
        /// <param name="code">Código de autorização enviado como parâmetro na URL</param>
        /// <param name="state">Estado (state) enviado como parâmetro na URL</param>
        /// <returns>Mensagem de sucesso ou erro</returns>
        [HttpGet]
        public async Task<IActionResult> GetAuthorizationCode([FromQuery] string code, [FromQuery] string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Authorization code não encontrado na URL.");
            }

            if (string.IsNullOrEmpty(state))
            {
                return BadRequest("State não encontrado ou inválido.");
            }

            // Log para depuração
            _logger.LogInformation($"Authorization Code: {code}");
            _logger.LogInformation($"State: {state}");

            try
            {

                // Chama o método da classe OAuthHelperGetTokens para obter o token
                var tokenResponse = await OAuthHelperGetTokens.GetAccessTokenAsync(code);

                tokenResponse = tokenResponse.Trim();

                if (string.IsNullOrEmpty(tokenResponse))
                {
                    _logger.LogError("Token Response está vazio ou nulo.");
                    return BadRequest("Token Response está vazio ou nulo.");
                }

                _logger.LogInformation($"Token Response: {tokenResponse}");                

                // Deserializar o JSON de resposta para um objeto TokenInfo
                var tokenInfo = JsonSerializer.Deserialize<TokenInfo>(tokenResponse);
                _logger.LogInformation("Token deserializado com sucesso.");                                             

                if (tokenInfo == null)
                {
                    //return BadRequest("TOKEN NULO: Não foi possível deserializar as informações do token.");
                }
                
                // Validações básicas no token
                if (string.IsNullOrEmpty(tokenInfo.AccessToken) || string.IsNullOrEmpty(tokenInfo.RefreshToken))
                {
                    return BadRequest(new { Message = "Token de acesso ou refresh token está vazio." });
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
                    TokenInfo = tokenInfo // Remova isso em produção
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
