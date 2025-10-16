using GCPIntellect.API.Data;
using GCPIntellect.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GCPIntellect.API.Controllers
{
    // Classe auxiliar (DTO) para receber os dados do login de forma segura.
    public class LoginModel
    {
        public string? Login { get; set; }
        public string? Senha { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")] // Rota será /api/auth
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        // Injetamos IConfiguration para ler as configurações do appsettings.json
        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")] // Rota final será POST /api/auth/login
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            // Adicionamos uma validação de entrada para garantir que os dados não são nulos ou vazios
            if (string.IsNullOrEmpty(loginModel.Login) || string.IsNullOrEmpty(loginModel.Senha))
            {
                return BadRequest("Login e Senha são obrigatórios.");
            }

            // 1. Busca o usuário no banco pelo login e verifica se está ativo
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Login == loginModel.Login && u.Ativo == true);

            if (usuario == null)
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            // 2. LÓGICA DE VERIFICAÇÃO DE SENHA HASH
            if (!VerificarSenhaHash(loginModel.Senha, usuario.SenhaHash))
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            // 3. LÓGICA DE GERAÇÃO DE TOKEN JWT
            var tokenString = GerarTokenJwt(usuario);

            // Retorna o token para o front-end
            return Ok(new { token = tokenString });
        }

        private bool VerificarSenhaHash(string senha, byte[] senhaHashArmazenada)
        {
            using (var sha256 = SHA256.Create())
            {
                // Gera o hash da senha que o usuário digitou
                var hashDaSenhaDigitada = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));

                // Compara os dois hashes. SequenceEqual é a forma correta de comparar arrays de bytes.
                return hashDaSenhaDigitada.SequenceEqual(senhaHashArmazenada);
            }
        }

        private string GerarTokenJwt(Usuario usuario)
        {
            // Pega a seção de configurações do JWT
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secret = jwtSettings["Secret"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            // Verificação para garantir que as configurações existem, evitando erros
            if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                throw new InvalidOperationException("Configurações do JWT (Secret, Issuer, Audience) não foram encontradas no appsettings.json.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(8);

            // "Claims" são as informações que queremos guardar dentro do token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Login ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.TipoAcesso ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}