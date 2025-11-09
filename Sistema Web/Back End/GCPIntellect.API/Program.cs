using GCPIntellect.API.Data;
using GCPIntellect.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GCPIntellect.API.Services.Interfaces; // Importa as interfaces

// --- 1. Configuração dos Serviços (Dependências) ---
var builder = WebApplication.CreateBuilder(args);

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra o DbContext
builder.Services.AddDbContext<ContextoBD>(options =>
    options.UseSqlServer(connectionString));

// Registra os Serviços customizados
builder.Services.AddScoped<ServicoGemini>();
builder.Services.AddScoped<IServicoEmail, ServicoEmail>();
builder.Services.AddScoped<IServicoNotificacao, ServicoNotificacao>();

// --- Configuração da Autenticação JWT ---
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"];
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("A chave secreta do JWT (JwtSettings:Secret) não está configurada.");
}
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});
builder.Services.AddAuthorization();
// --- FIM DA AUTENTICAÇÃO ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- 2. Construção da Aplicação ---
var app = builder.Build();

// --- 3. Configuração do Pipeline de Requisições HTTP ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- CORREÇÃO (ADICIONADO) ---
// Habilita o servidor para entregar arquivos estáticos (CSS, JS, e seus UPLOADS)
// da pasta wwwroot.
app.UseStaticFiles();
// --- FIM DA CORREÇÃO ---

// Aplica a política de CORS
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// --- 4. Inicialização do Banco de Dados (Seed) ---
try
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ContextoBD>();
        SeedData.Initialize(context); // Garante que o usuário 'admin' exista
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Ocorreu um erro durante a inicialização (Seed) do banco de dados.");
}

// --- 5. Execução da Aplicação ---
app.Run();