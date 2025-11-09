using GCPIntellect.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GCPIntellect.API.Controllers
{
    [ApiController]
    [Route("api/tipos")] // <-- CORREÇÃO: Rota explícita para o frontend
    [Authorize]
    public class ControladorTipos : ControllerBase // Classe Renomeada
    {
        private readonly ContextoBD _contexto;

        public ControladorTipos(ContextoBD contexto)
        {
            _contexto = contexto;
        }

        // GET: api/tipos
        [HttpGet]
        public async Task<IActionResult> ObterTodos() // Método Renomeado
        {
            var tipos = await _contexto.Tipos
                .OrderBy(t => t.Nome) // Ordenado por Nome
                .Select(t => new { t.Id, t.Nome })
                .ToListAsync();

            return Ok(tipos);
        }
    }
}