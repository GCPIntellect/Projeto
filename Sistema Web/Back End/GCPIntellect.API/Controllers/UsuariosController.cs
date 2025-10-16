using GCPIntellect.API.Data;
using GCPIntellect.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace GCPIntellect.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Define a rota base como /api/usuarios
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // O DbContext é injetado aqui, nos dando acesso ao banco de dados.
        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- MÉTODOS DA API ---

        // GET: api/usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            // Busca todos os usuários no banco de forma assíncrona.
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            // Busca um usuário específico pelo seu Id.
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                // Retorna um erro 404 Not Found se o usuário não for encontrado.
                return NotFound();
            }

            return Ok(usuario);
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            // IMPORTANTE: A senha NUNCA deve chegar como hash do front-end.
            // Aqui, estamos assumindo que o front-end enviou uma senha em texto puro
            // e o back-end é responsável por criar o hash.
            // A lógica real de receber um "DTO" (Data Transfer Object) com a senha é mais complexa.
            // Por simplicidade, vamos gerar um hash de exemplo.
            
            // Lógica para criar um hash da senha (exemplo simples, não use em produção)
            // Em um projeto real, você receberia um objeto com a senha em texto e a converteria aqui.
            using (var sha256 = SHA256.Create())
            {
                // Simulando que recebemos uma senha "senha123"
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes("senha123"));
                usuario.SenhaHash = hashedBytes;
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Retorna um status 201 Created com a localização do novo recurso e o objeto criado.
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        // PUT: api/usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                // Retorna um erro 400 Bad Request se os Ids não baterem.
                return BadRequest();
            }

            // Informa ao Entity Framework que este objeto foi modificado.
            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Usuarios.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Retorna um status 204 No Content, indicando sucesso na atualização.
            return NoContent();
        }

        // DELETE: api/usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // Lógica de "Soft Delete": Em vez de apagar, apenas desativamos o usuário.
            // Isso preserva todo o histórico de chamados e outras interações dele.
            usuario.Ativo = false;
            _context.Entry(usuario).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}