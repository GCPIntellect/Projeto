using GCPIntellect.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GCPIntellect.API.Controllers
{
    [ApiController]
    [Route("api/status")] // <-- CORREÇÃO: Rota explícita para o frontend
    [Authorize]
    public class ControladorStatus : ControllerBase // Classe Renomeada
    {
        private readonly ContextoBD _contexto;

        public ControladorStatus(ContextoBD contexto)
        {
            _contexto = contexto;
        }

        // GET: api/status
        [HttpGet]
        public async Task<IActionResult> ObterTodos() // Método Renomeado
        {
            var status = await _contexto.StatusChamados
                .OrderBy(s => s.Id)
                .Select(s => new { s.Id, s.Nome })
                .ToListAsync();

            return Ok(status);
        }
    }
}