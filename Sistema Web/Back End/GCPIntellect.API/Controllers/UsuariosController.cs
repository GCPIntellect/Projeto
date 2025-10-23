// ARQUIVO: Controllers/UsuariosController.cs (VERSÃO CORRIGIDA E SEGURA)

using GCPIntellect.API.Data;
using GCPIntellect.API.Models;
using Microsoft.AspNetCore.Authorization; // Necessário para proteção
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GCPIntellect.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")] // <-- IMPORTANTE: Protege todos os endpoints deste controller
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/usuarios
        // Apenas administradores logados podem acessar esta lista
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            // Ocultamos o hash da senha na resposta por segurança
            var usuarios = await _context.Usuarios
                .Select(u => new 
                { 
                    u.Id, 
                    u.Nome, 
                    u.Login, 
                    u.Email, 
                    u.TipoAcesso, 
                    u.DataCadastro, 
                    u.Ativo, 
                    u.Telefone 
                })
                .ToListAsync();
            return Ok(usuarios);
        }

        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // Ocultamos o hash da senha na resposta
            usuario.SenhaHash = null!; 
            return Ok(usuario);
        }
        
        // PUT: api/usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            // Garante que a senha não seja alterada por este método
            _context.Entry(usuario).Property(x => x.SenhaHash).IsModified = false;
            _context.Entry(usuario).State = EntityState.Modified;
            
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/usuarios/5 (Soft Delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Ativo = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}