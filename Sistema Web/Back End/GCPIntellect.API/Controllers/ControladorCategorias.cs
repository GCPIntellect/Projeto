using GCPIntellect.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GCPIntellect.API.Controllers
{
    [ApiController]
    [Route("api/categorias")] // <-- CORREÇÃO: Rota explícita para o frontend
    [Authorize]
    public class ControladorCategorias : ControllerBase // Classe Renomeada
    {
        private readonly ContextoBD _contexto;

        public ControladorCategorias(ContextoBD contexto)
        {
            _contexto = contexto;
        }

        // GET: api/categorias
        [HttpGet]
        public async Task<IActionResult> ObterTodas() // Método Renomeado
        {
            var categorias = await _contexto.Categorias
                .OrderBy(c => c.Nome) // Ordenado por nome
                .Select(c => new { c.Id, c.Nome })
                .ToListAsync();

            return Ok(categorias);
        }
    }
}