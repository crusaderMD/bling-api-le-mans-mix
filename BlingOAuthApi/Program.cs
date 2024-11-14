using Microsoft.EntityFrameworkCore;
using BlingApiDailyConsult.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Adicionado suporte a appsettings.json
var configuration = builder.Configuration;

// Registra a ConnectionString no servi�o DI (Dependency Injection) se necess�rio
builder.Services.AddSingleton<IConfiguration>(configuration);

// Registrando no container de servi�os
builder.Services.AddTransient<DataBaseHelper>();

// Adicionado suporte a controladores
builder.Services.AddControllers();

// Adicionado Servi�os
builder.Services.AddScoped<OAuthHelperGetTokens>(); // Registra o OAuthHelperGetTokens

// Opcional: Swagger para documenta��o da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure o pipeline de requisi��es
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Descomente se HTTPS for necess�rio
// app.UseHttpsRedirection();

// Configurar para usar controladores
app.MapControllers();

app.Run();
