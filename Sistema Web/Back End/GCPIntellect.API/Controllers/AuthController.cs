using GCPIntellect.API.Data;
using GCPIntellect.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GCPIntellect.API.Controllers
{
    // DTO para receber dados do login
    public class LoginModel { public string? Login { get; set; } public string? Senha { get; set; } }

    // DTO para receber dados da solicitação de reset
    public class ResetRequestModel { public string? LoginOuEmail { get; set; } }

    [ApiController]
    [Route("api/[controller]")] // Rota base será /api/auth
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // --- ENDPOINT DE LOGIN ---
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.Login) || string.IsNullOrEmpty(loginModel.Senha))
            {
                return BadRequest("Login e Senha são obrigatórios.");
            }
            
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Login == loginModel.Login && u.Ativo == true);

            if (usuario == null || !VerificarSenhaHash(loginModel.Senha, usuario.SenhaHash))
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            var tokenString = GerarTokenJwt(usuario);
            return Ok(new { token = tokenString });
        }

        // --- ENDPOINT DE SOLICITAÇÃO DE RESET DE SENHA (NOVO) ---
        [HttpPost("solicitar-reset")]
        public async Task<IActionResult> SolicitarResetSenha([FromBody] ResetRequestModel request)
        {
            if (string.IsNullOrEmpty(request.LoginOuEmail))
            {
                return BadRequest();
            }

            // 1. Encontra o usuário que está pedindo o reset
            var usuarioSolicitante = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Login == request.LoginOuEmail || u.Email == request.LoginOuEmail);

            if (usuarioSolicitante == null)
            {
                // Mesmo se o usuário não existir, retornamos sucesso para não dar dicas a invasores.
                return Ok(new { message = "Se uma conta com este identificador existir, uma notificação foi enviada ao administrador." });
            }

            // 2. Encontra todos os administradores ativos para notificá-los
            var administradores = await _context.Usuarios
                .Where(u => u.TipoAcesso == "Administrador" && u.Ativo == true)
                .ToListAsync();

            // 3. Cria uma tarefa de notificação por e-mail para cada administrador
            foreach (var admin in administradores)
            {
                if (!string.IsNullOrEmpty(admin.Email))
                {
                    var novaNotificacao = new NotificacaoFila
                    {
                        IdChamado = null, // Não está ligado a um chamado
                        TipoNotificacao = "EMAIL",
                        Destinatario = admin.Email,
                        Assunto = "Solicitação de Redefinição de Senha",
                        Conteudo = $"O usuário '{usuarioSolicitante.Nome}' (Login: {usuarioSolicitante.Login}) solicitou uma redefinição de senha. Por favor, acesse o painel de gerenciamento de usuários para definir uma nova senha temporária para ele.",
                        Status = "Pendente"
                    };
                    _context.NotificacaoFilas.Add(novaNotificacao);
                }
            }

            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Se uma conta com este identificador existir, uma notificação foi enviada ao administrador." });
        }


        // --- FUNÇÕES AUXILIARES ---
          private bool VerificarSenhaHash(string senha, byte[] senhaHashArmazenada)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashDaSenhaDigitada = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha.Trim()));
                return hashDaSenhaDigitada.SequenceEqual(senhaHashArmazenada);
            }
        }

        private string GerarTokenJwt(Usuario usuario)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secret = jwtSettings["Secret"];
            if (string.IsNullOrEmpty(secret)) throw new InvalidOperationException("JWT Secret não encontrado.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(8);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.TipoAcesso ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Login ?? string.Empty)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}