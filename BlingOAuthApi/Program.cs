using Microsoft.EntityFrameworkCore;
using BlingApiDailyConsult.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Adicionado suporte a appsettings.json
var configuration = builder.Configuration;

// Registra a ConnectionString no serviço DI (Dependency Injection) se necessário
builder.Services.AddSingleton<IConfiguration>(configuration);

// Registrando no container de serviços
builder.Services.AddTransient<DataBaseHelper>();

// Adicionado suporte a controladores
builder.Services.AddControllers();

// Adicionado Serviços
builder.Services.AddScoped<OAuthHelperGetTokens>(); // Registra o OAuthHelperGetTokens

// Opcional: Swagger para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure o pipeline de requisições
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Descomente se HTTPS for necessário
// app.UseHttpsRedirection();

// Configurar para usar controladores
app.MapControllers();

app.Run();
