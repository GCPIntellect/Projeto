using GCPIntellect.API.Data;
using GCPIntellect.API.Models;
using GCPIntellect.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GCPIntellect.API.Controllers
{
    public class ConsultaModel { public string Pergunta { get; set; } = string.Empty; }
    public class FeedbackModel { public bool FoiUtil { get; set; } }

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly GeminiService _geminiService;

        public IaController(ApplicationDbContext context, GeminiService geminiService)
        {
            _context = context;
            _geminiService = geminiService;
        }

        [HttpPost("consulta")]
        public async Task<IActionResult> ConsultarIa([FromBody] ConsultaModel model)
        {
            var idUsuarioLogadoClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idUsuarioLogadoClaim, out var idUsuarioLogado)) return Unauthorized();

            string solucao;
            int? idBaseConhecimento = null;

            var palavrasChaveBusca = model.Pergunta.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                      .Where(p => p.Length > 3).ToList();
            
            var artigo = await _context.BaseConhecimentos
                .Include(b => b.PalavrasChave)
                .ThenInclude(p => p.PalavraChave)
                .FirstOrDefaultAsync(b => 
                    b.Titulo.ToLower().Contains(model.Pergunta.ToLower()) || 
                    b.PalavrasChave.Any(p => p.PalavraChave != null && palavrasChaveBusca.Contains(p.PalavraChave.Texto.ToLower()))
                );

            if (artigo != null)
            {
                solucao = artigo.Resposta;
                idBaseConhecimento = artigo.Id;
            }
            else
            {
                solucao = await _geminiService.GerarSolucaoAsync(model.Pergunta);
            }

            var novaConsulta = new ConsultaIA
            {
                IdUsuario = idUsuarioLogado,
                PerguntaUsuario = model.Pergunta,
                RespostaGerada = solucao,
                IdBaseConhecimentoSugerido = idBaseConhecimento,
                IdSessao = Guid.NewGuid().ToString()
            };
            _context.ConsultasIA.Add(novaConsulta);
            await _context.SaveChangesAsync();

            return Ok(new { solucao, idConsulta = novaConsulta.Id });
        }
        
        // ... (resto do controller, como o RegistrarFeedback, continua o mesmo)
    }
}