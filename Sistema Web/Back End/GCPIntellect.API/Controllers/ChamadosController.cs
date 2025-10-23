using GCPIntellect.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GCPIntellect.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChamadosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChamadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- ENDPOINT PARA BUSCAR OS CHAMADOS COM FILTROS ---
        [HttpGet]
        public async Task<IActionResult> GetChamados(
            [FromQuery] int? statusId, 
            [FromQuery] int? prioridadeId, 
            [FromQuery] int? tipoId,
            [FromQuery] DateTime? dataInicio,
            [FromQuery] DateTime? dataFim)
        {
            // Começa com uma consulta base que será filtrada dinamicamente
            var query = _context.Chamados.AsQueryable();

            var tipoAcesso = User.FindFirstValue(ClaimTypes.Role);

            if (tipoAcesso == "Colaborador")
            {
                // --- INÍCIO DA CORREÇÃO ---
                // Verifica de forma segura se o ID do usuário existe no token
                var idUsuarioLogadoClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(idUsuarioLogadoClaim) || !int.TryParse(idUsuarioLogadoClaim, out var idUsuarioLogado))
                {
                    // Se não for possível obter o ID, retorna "Não Autorizado"
                    return Unauthorized("Não foi possível identificar o usuário a partir do token.");
                }
                // --- FIM DA CORREÇÃO ---

                query = query.Where(c => c.IdUsuario == idUsuarioLogado);
            }
            
            // Aplica filtros opcionais da URL
            if (statusId.HasValue) query = query.Where(c => c.IdStatus == statusId.Value);
            if (prioridadeId.HasValue) query = query.Where(c => c.IdPrioridade == prioridadeId.Value);
            if (tipoId.HasValue) query = query.Where(c => c.IdTipo == tipoId.Value);
            if (dataInicio.HasValue) query = query.Where(c => c.DataAbertura >= dataInicio.Value);
            if (dataFim.HasValue) query = query.Where(c => c.DataAbertura < dataFim.Value.AddDays(1));

            // Executa a consulta final
            var chamados = await query
                .Include(c => c.Usuario)
                .Include(c => c.StatusChamado)
                .Include(c => c.Prioridade)
                .OrderByDescending(c => c.DataAbertura)
                // Seleciona apenas os campos necessários para a lista (mais eficiente)
                .Select(c => new { 
                    c.Id,
                    c.Titulo,
                    Usuario = new { c.Usuario.Nome },
                    StatusChamado = new { c.StatusChamado.Nome },
                    Prioridade = new { c.Prioridade.Nome },
                    c.DataAbertura
                })
                .ToListAsync();

            return Ok(chamados);
        }
    }
}