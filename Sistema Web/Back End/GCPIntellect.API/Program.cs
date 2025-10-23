using GCPIntellect.API.Data;
using GCPIntellect.API.Services; // <-- 1. ADICIONADO: using para encontrar o GeminiService
using Microsoft.EntityFrameworkCore;

// --- 1. Configuração dos Serviços ---
var builder = WebApplication.CreateBuilder(args);

// Adiciona o serviço de CORS
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

// Pega a string de conexão
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra o DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Registra o GeminiService (para Injeção de Dependência)
builder.Services.AddScoped<GeminiService>(); // <-- 2. ADICIONADO: Registra o serviço da IA

// Adiciona os serviços do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adiciona o serviço dos Controllers
builder.Services.AddControllers();


// --- 2. Construção da Aplicação ---
var app = builder.Build();


// --- 3. Configuração do Pipeline de Requisições HTTP ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Cuidado com HTTPS em desenvolvimento local se não tiver certificado confiável

app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();


// --- 4. Execução da Aplicação ---
app.Run();