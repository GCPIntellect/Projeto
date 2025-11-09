using GCPIntellect.API.Data;
using GCPIntellect.API.Models;
using GCPIntellect.API.Models.Enums;
using GCPIntellect.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GCPIntellect.API.Controllers
{
    [ApiController]
    [Route("api/auth")] // Rota base /api/auth
    public class AutenticacaoController : ControllerBase
    {
        private readonly ContextoBD _contexto;
        private readonly IConfiguration _configuracao;

        public AutenticacaoController(ContextoBD contexto, IConfiguration configuracao)
        {
            _contexto = contexto;
            _configuracao = configuracao;
        }

        // --- ENDPOINT DE LOGIN ---
        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ModeloLogin modeloLogin)
        {
            if (modeloLogin == null || string.IsNullOrEmpty(modeloLogin.Login) || string.IsNullOrEmpty(modeloLogin.Senha))
            {
                return BadRequest("Login e Senha são obrigatórios.");
            }

            // 1. Encontra o usuário ativo pelo login
            var usuario = await _contexto.Usuarios
                .FirstOrDefaultAsync(u => u.Login == modeloLogin.Login && u.Ativo == true);

            // 2. Verifica se o usuário existe e se a senha está correta
            // A lógica de verificação da senha está agora dentro do modelo UsuarioBase (OO)
            if (usuario == null || !usuario.VerificarSenha(modeloLogin.Senha))
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            // 3. Gera o token JWT
            var tokenString = GerarTokenJwt(usuario);
            return Ok(new { token = tokenString });
        }

        // --- ENDPOINT DE SOLICITAÇÃO DE RESET DE SENHA ---
        // POST: api/auth/solicitar-reset
        [HttpPost("solicitar-reset")]
        public async Task<IActionResult> SolicitarResetSenha([FromBody] ModeloResetSenha request)
        {
            if (string.IsNullOrEmpty(request.LoginOuEmail))
            {
                return BadRequest("Login ou Email é obrigatório.");
            }

            // 1. Encontra o usuário que está pedindo o reset
            var usuarioSolicitante = await _contexto.Usuarios
                .FirstOrDefaultAsync(u => u.Login == request.LoginOuEmail || u.Email == request.LoginOuEmail);

            if (usuarioSolicitante == null)
            {
                // Mesmo se o usuário não existir, retornamos sucesso para não dar dicas a invasores.
                return Ok(new { message = "Se uma conta com este identificador existir, uma notificação foi enviada ao administrador." });
            }

            // 2. Encontra todos os administradores ativos (Usando o TPH com OfType<>)
            var administradores = await _contexto.Usuarios
                .OfType<Administrador>()
                .Where(u => u.Ativo == true)
                .ToListAsync();

            // 3. Cria uma tarefa de notificação por e-mail para cada administrador
            foreach (var admin in administradores)
            {
                if (!string.IsNullOrEmpty(admin.Email))
                {
                    var novaNotificacao = new NotificacaoFila
                    {
                        IdChamado = null, // Não está ligado a um chamado
                        TipoNotificacao = TipoNotificacao.EMAIL,
                        Destinatario = admin.Email,
                        Assunto = "Solicitação de Redefinição de Senha",
                        Conteudo = $"O usuário '{usuarioSolicitante.Nome}' (Login: {usuarioSolicitante.Login}) solicitou uma redefinição de senha. Por favor, acesse o painel de gerenciamento de usuários para definir uma nova senha temporária para ele.",
                        Status = StatusNotificacao.Pendente
                    };
                    _contexto.NotificacoesFila.Add(novaNotificacao);
                }
            }

            await _contexto.SaveChangesAsync();
            
            return Ok(new { message = "Se uma conta com este identificador existir, uma notificação foi enviada ao administrador." });
        }

        // --- FUNÇÕES AUXILIARES ---

        /// <summary>
        /// Gera um token JWT para um usuário autenticado.
        /// </summary>
        /// <param name="usuario">O objeto do usuário (pode ser Administrador, Tecnico ou Colaborador).</param>
        /// <returns>Uma string de token JWT.</returns>
        private string GerarTokenJwt(UsuarioBase usuario)
        {
            var jwtSettings = _configuracao.GetSection("JwtSettings");
            var secret = jwtSettings["Secret"];
            if (string.IsNullOrEmpty(secret)) throw new InvalidOperationException("JWT Secret não configurado.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(8); // Duração do token

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.ObterTipoAcesso()), // Usa polimorfismo para obter o "Role"
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Login)
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