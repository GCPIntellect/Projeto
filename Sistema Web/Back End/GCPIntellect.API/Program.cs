// ===================================================================
// ARQUIVO: Program.cs (VERSÃO FINAL E CORRETA)
// ===================================================================

// --- Importações necessárias (using) ---
using GCPIntellect.API.Data;
using Microsoft.EntityFrameworkCore;

// --- 1. Configuração dos Serviços ---
var builder = WebApplication.CreateBuilder(args);

// Adiciona o serviço de CORS para permitir a comunicação com o Front-end
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Pega a string de conexão do arquivo appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra o DbContext para a conexão com o banco de dados SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Adiciona os serviços do Swagger para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adiciona o serviço dos Controllers (para encontrar nossas classes de API)
builder.Services.AddControllers();


// --- 2. Construção da Aplicação ---
var app = builder.Build();


// --- 3. Configuração do Pipeline de Requisições HTTP ---
// Define o que a aplicação faz quando recebe uma requisição. A ORDEM IMPORTA.

// Em ambiente de desenvolvimento, habilita a interface do Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redireciona requisições HTTP para HTTPS
app.UseHttpsRedirection();

// Aplica a política de CORS que definimos
app.UseCors("AllowAllOrigins");

// Habilita a autorização (usaremos com JWT no futuro)
app.UseAuthorization();

// Mapeia as rotas para os nossos Controllers
app.MapControllers();


// --- 4. Execução da Aplicação ---
app.Run();