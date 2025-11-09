using GCPIntellect.API.Data;
using GCPIntellect.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GCPIntellect.API.Controllers
{
    [ApiController]
    [Route("api/usuarios")] // Rota base /api/usuarios (correta para o frontend)
    [Authorize(Roles = "Administrador")] // Apenas Admins podem gerenciar usuários
    public class ControladorUsuarios : ControllerBase // Classe Renomeada
    {
        private readonly ContextoBD _contexto;

        public ControladorUsuarios(ContextoBD contexto)
        {
            _contexto = contexto;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioBase>>> ObterUsuarios() // Método Renomeado
        {
            var usuarios = await _contexto.Usuarios
                .Select(u => new 
                { 
                    u.Id, 
                    u.Nome, 
                    u.Login, 
                    u.Email, 
                    TipoAcesso = u.ObterTipoAcesso(), // CORREÇÃO: Usando o método polimórfico
                    u.DataCadastro, 
                    u.Ativo, 
                    u.Telefone 
                })
                .ToListAsync();
            return Ok(usuarios);
        }

        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioBase>> ObterUsuarioPorId(int id) // Método Renomeado
        {
            var usuario = await _contexto.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            usuario.SenhaHash = null!; // Nunca retornar o hash da senha
            return Ok(usuario);
        }
        
        // PUT: api/usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUsuario(int id, [FromBody] UsuarioBase usuario) // Método Renomeado
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            // Garante que a senha não seja alterada por este método
            _contexto.Entry(usuario).Property(x => x.SenhaHash).IsModified = false;
            _contexto.Entry(usuario).State = EntityState.Modified;
            
            await _contexto.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/usuarios/5 (Soft Delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirUsuario(int id) // Método Renomeado
        {
            var usuario = await _contexto.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // Soft delete
            usuario.Ativo = false;
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}