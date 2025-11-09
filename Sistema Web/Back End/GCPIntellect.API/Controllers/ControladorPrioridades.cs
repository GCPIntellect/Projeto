using GCPIntellect.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GCPIntellect.API.Controllers
{
    [ApiController]
    [Route("api/prioridades")] // <-- CORREÇÃO: Rota explícita para o frontend
    [Authorize]
    public class ControladorPrioridades : ControllerBase // Classe Renomeada
    {
        private readonly ContextoBD _contexto;

        public ControladorPrioridades(ContextoBD contexto)
        {
            _contexto = contexto;
        }

        // GET: api/prioridades
        [HttpGet]
        public async Task<IActionResult> ObterTodas() // Método Renomeado
        {
            var prioridades = await _contexto.Prioridades
                .OrderBy(p => p.Id) // Ordenado por ID (Baixo, Medio, Alto...)
                .Select(p => new { p.Id, p.Nome })
                .ToListAsync();

            return Ok(prioridades);
        }
    }
}