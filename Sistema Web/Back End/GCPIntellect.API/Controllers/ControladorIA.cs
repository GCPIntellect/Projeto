using GCPIntellect.API.Data;
using GCPIntellect.API.Models;
using GCPIntellect.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GCPIntellect.API.Controllers
{
    // DTOs internos renomeados
    public class ModeloConsulta { public string Pergunta { get; set; } = string.Empty; }
    public class ModeloFeedback { public bool FoiUtil { get; set; } }

    [ApiController]
    [Route("api/ia")] // Rota base /api/ia (correta para o frontend)
    [Authorize]
    public class ControladorIA : ControllerBase // Classe Renomeada
    {
        private readonly ContextoBD _contexto;
        private readonly ServicoGemini _servicoGemini;

        public ControladorIA(ContextoBD contexto, ServicoGemini servicoGemini)
        {
            _contexto = contexto;
            _servicoGemini = servicoGemini;
        }

        // POST: api/ia/consulta
        [HttpPost("consulta")]
        public async Task<IActionResult> ConsultarIA([FromBody] ModeloConsulta model)
        {
            var idUsuarioLogadoClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idUsuarioLogadoClaim, out var idUsuarioLogado)) return Unauthorized();

            string solucao;
            int? idBaseConhecimento = null;

            // 1. Tenta encontrar na base de conhecimento local
            var palavrasChaveBusca = model.Pergunta.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                        .Where(p => p.Length > 3).ToList();
            
            var artigo = await _contexto.BaseConhecimentos
                .Include(b => b.PalavrasChave)
                .ThenInclude(bcp => bcp.PalavraChave)
                .FirstOrDefaultAsync(b => 
                    b.Titulo.ToLower().Contains(model.Pergunta.ToLower()) || 
                    b.PalavrasChave.Any(p => p.PalavraChave != null && palavrasChaveBusca.Contains(p.PalavraChave.Texto.ToLower()))
                );

            // 2. Decide a fonte da solução
            if (artigo != null)
            {
                solucao = artigo.Resposta;
                idBaseConhecimento = artigo.Id;
            }
            else
            {
                // 3. Se não achar, consulta a IA (Gemini)
                solucao = await _servicoGemini.GerarSolucaoAsync(model.Pergunta);
            }

            // 4. Salva o log da consulta
            var novaConsulta = new ConsultaIA
            {
                IdUsuario = idUsuarioLogado,
                PerguntaUsuario = model.Pergunta,
                RespostaGerada = solucao,
                IdBaseConhecimentoSugerido = idBaseConhecimento,
                IdSessao = Guid.NewGuid().ToString() // (Ainda não implementado no frontend)
            };
            _contexto.ConsultasIA.Add(novaConsulta);
            await _contexto.SaveChangesAsync();

            return Ok(new { solucao, idConsulta = novaConsulta.Id });
        }

        // POST: api/ia/consulta/{idConsulta}/feedback
        [HttpPost("consulta/{idConsulta}/feedback")]
        public async Task<IActionResult> RegistrarFeedback(int idConsulta, [FromBody] ModeloFeedback model)
        {
            var consulta = await _contexto.ConsultasIA.FindAsync(idConsulta);
            if (consulta == null) return NotFound();

            consulta.FoiUtil = model.FoiUtil;
            await _contexto.SaveChangesAsync();

            return Ok();
        }
    }
}