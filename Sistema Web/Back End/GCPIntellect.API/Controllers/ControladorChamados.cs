using GCPIntellect.API.Data;
using GCPIntellect.API.Models;
using GCPIntellect.API.Services.Interfaces;
using GCPIntellect.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting; // 1. ADICIONADO
using System.IO; // 2. ADICIONADO

namespace GCPIntellect.API.Controllers
{
    [ApiController]
    [Route("api/chamados")]
    [Authorize]
    public class ControladorChamados : ControllerBase
    {
        private readonly ContextoBD _contexto;
        private readonly IServicoEmail _servicoEmail;
        private readonly IWebHostEnvironment _webHostEnvironment; // 3. ADICIONADO

        // 4. CONSTRUTOR ATUALIZADO
        public ControladorChamados(ContextoBD contexto, IServicoEmail servicoEmail, IWebHostEnvironment webHostEnvironment) 
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
            _servicoEmail = servicoEmail ?? throw new ArgumentNullException(nameof(servicoEmail));
            _webHostEnvironment = webHostEnvironment; // 5. ADICIONADO
        }

        // GET: api/chamados
        [HttpGet]
        public async Task<IActionResult> ObterChamados(
            [FromQuery] int? statusId, [FromQuery] int? prioridadeId, [FromQuery] int? tipoId,
            [FromQuery] DateTime? dataInicio, [FromQuery] DateTime? dataFim,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            var consulta = _contexto.Chamados.AsQueryable();
            var tipoAcesso = User.FindFirstValue(ClaimTypes.Role);

            if (tipoAcesso == "Colaborador")
            {
                var idUsuarioLogadoClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(idUsuarioLogadoClaim, out var idUsuarioLogado)) return Unauthorized();
                consulta = consulta.Where(c => c.IdUsuario == idUsuarioLogado);
            }
            
            if (statusId.HasValue) consulta = consulta.Where(c => c.IdStatus == statusId.Value);
            if (prioridadeId.HasValue) consulta = consulta.Where(c => c.IdPrioridade == prioridadeId.Value);
            if (tipoId.HasValue) consulta = consulta.Where(c => c.IdTipo == tipoId.Value);
            if (dataInicio.HasValue) consulta = consulta.Where(c => c.DataAbertura >= dataInicio.Value);
            if (dataFim.HasValue) consulta = consulta.Where(c => c.DataAbertura < dataFim.Value.AddDays(1));

            var totalItens = await consulta.CountAsync();

            var chamadosPaginados = await consulta
                .Include(c => c.Usuario)
                .Include(c => c.StatusChamado)
                .Include(c => c.Prioridade)
                .OrderByDescending(c => c.DataAbertura)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .Select(c => new {
                    c.Id,
                    c.Titulo,
                    c.IdUsuario,
                    Usuario = new { Nome = c.Usuario != null ? c.Usuario.Nome : "N/A" },
                    StatusChamado = new { Nome = c.StatusChamado != null ? c.StatusChamado.Nome : "N/A" },
                    Prioridade = new { Nome = c.Prioridade != null ? c.Prioridade.Nome : "N/A" },
                    c.DataAbertura
                })
                .ToListAsync();
            
            var resposta = new
            {
                TotalItens = totalItens,
                TotalPaginas = (int)Math.Ceiling(totalItens / (double)tamanhoPagina),
                PaginaAtual = pagina,
                TamanhoPagina = tamanhoPagina,
                Dados = chamadosPaginados
            };

            return Ok(resposta);
        }

        // GET: api/chamados/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterChamadoPorId(int id)
        {
            var idUsuarioLogadoClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tipoAcesso = User.FindFirstValue(ClaimTypes.Role);
            if (!int.TryParse(idUsuarioLogadoClaim, out var idUsuarioLogado)) return Unauthorized();

            var chamado = await _contexto.Chamados
                .AsNoTracking()
                .Include(c => c.Usuario)
                .Include(c => c.StatusChamado)
                .Include(c => c.Prioridade)
                .Include(c => c.Tipo)
                .Include(c => c.Categoria)
                .Include(c => c.Mensagens).ThenInclude(m => m.Autor)
                .Include(c => c.Anexos)
                .Include(c => c.TecnicosAtribuidos).ThenInclude(ct => ct.Tecnico)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (chamado == null) return NotFound("Chamado não encontrado.");

            if (tipoAcesso == "Colaborador" && chamado.IdUsuario != idUsuarioLogado)
            {
                return Forbid("Acesso negado. Você só pode ver seus próprios chamados.");
            }

            var viewModel = new ChamadoDetalheVisaoModel
            {
                Id = chamado.Id,
                Titulo = chamado.Titulo,
                Descricao = chamado.Descricao,
                DataAbertura = chamado.DataAbertura,
                IdUsuario = chamado.IdUsuario,
                IdStatus = chamado.IdStatus,
                IdCategoria = chamado.IdCategoria,
                IdTipo = chamado.IdTipo,
                IdPrioridade = chamado.IdPrioridade,

                Usuario = chamado.Usuario != null ? new UsuarioVisaoModel { Id = chamado.Usuario.Id, Nome = chamado.Usuario.Nome } : null,
                StatusChamado = chamado.StatusChamado != null ? new TabelaAuxiliarVisaoModel { Id = chamado.StatusChamado.Id, Nome = chamado.StatusChamado.Nome } : null,
                Categoria = chamado.Categoria != null ? new TabelaAuxiliarVisaoModel { Id = chamado.Categoria.Id, Nome = chamado.Categoria.Nome } : null,
                Tipo = chamado.Tipo != null ? new TabelaAuxiliarVisaoModel { Id = chamado.Tipo.Id, Nome = chamado.Tipo.Nome } : null,
                Prioridade = chamado.Prioridade != null ? new TabelaAuxiliarVisaoModel { Id = chamado.Prioridade.Id, Nome = chamado.Prioridade.Nome } : null,

                Mensagens = (chamado.Mensagens ?? new List<ChamadoMensagem>()).Select(m => new MensagemVisaoModel
                {
                    Id = m.Id,
                    Conteudo = m.Conteudo,
                    DataEnvio = m.DataEnvio,
                    Autor = m.Autor != null ? new UsuarioVisaoModel { Id = m.Autor.Id, Nome = m.Autor.Nome } : null
                }).ToList(),

                Anexos = (chamado.Anexos ?? new List<Anexo>()).Select(a => new AnexoVisaoModel
                {
                    Id = a.Id,
                    NomeArquivo = a.NomeArquivo,
                    CaminhoArquivo = a.CaminhoArquivo,
                    TamanhoBytes = a.TamanhoBytes
                }).ToList(),

                TecnicosAtribuidos = (chamado.TecnicosAtribuidos ?? new List<ChamadoTecnico>())
                    .Where(t => t.Tecnico != null)
                    .Select(t => new UsuarioVisaoModel
                    {
                        Id = t.Tecnico!.Id,
                        Nome = t.Tecnico.Nome
                    }).ToList()
            };

            return Ok(viewModel);
        }

        // POST: api/chamados
        [HttpPost]
        public async Task<IActionResult> CriarChamado([FromForm] ChamadoCriacaoModel model, [FromForm] List<IFormFile> anexos)
        {
            var idUsuarioLogadoClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idUsuarioLogadoClaim, out var idUsuarioLogado)) return Unauthorized();

            var usuarioLogado = await _contexto.Usuarios.FindAsync(idUsuarioLogado);
            if (usuarioLogado == null) return Unauthorized("Usuário não encontrado.");

            var statusPadrao = await _contexto.StatusChamados.FirstOrDefaultAsync(s => s.Nome == "Em Andamento");
            var prioridadePadrao = await _contexto.Prioridades.FirstOrDefaultAsync(p => p.Nome == "Baixo");
            if (statusPadrao == null || prioridadePadrao == null)
            {
                return StatusCode(500, "Erro: Status ou Prioridade padrão não encontrados no banco.");
            }

            var novoChamado = new Chamado
            {
                Titulo = model.Titulo,
                Descricao = model.Descricao,
                IdTipo = model.IdTipo,
                IdCategoria = model.IdCategoria,
                IdUsuario = idUsuarioLogado,
                DataAbertura = DateTime.UtcNow
            };

            usuarioLogado.DefinirStatusEPrioridade(novoChamado, model, prioridadePadrao, statusPadrao);

            _contexto.Chamados.Add(novoChamado);
            await _contexto.SaveChangesAsync();  // Salva o chamado para obter o novoChamado.Id

            // --- INÍCIO DA CORREÇÃO DE UPLOAD ---
            if (anexos != null && anexos.Count > 0)
            {
                // Define o caminho físico para a pasta "uploads" dentro de "wwwroot"
                var uploadsFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                // Garante que o diretório exista
                Directory.CreateDirectory(uploadsFolderPath);

                foreach (var anexoFile in anexos)
                {
                    // Gera um nome único para evitar sobreposição de arquivos
                    var uniqueFileName = $"{Guid.NewGuid()}_{anexoFile.FileName}";
                    // O caminho relativo que será salvo no banco (ex: "uploads/meuarquivo.pdf")
                    var caminhoParaSalvar = Path.Combine("uploads", uniqueFileName).Replace("\\", "/");
                    // O caminho físico completo no disco (ex: "C:/.../wwwroot/uploads/meuarquivo.pdf")
                    var physicalPath = Path.Combine(uploadsFolderPath, uniqueFileName);

                    // Salva o arquivo fisicamente no disco
                    await using (var stream = new FileStream(physicalPath, FileMode.Create))
                    {
                        await anexoFile.CopyToAsync(stream);
                    }

                    var novoAnexo = new Anexo
                    {
                        IdChamado = novoChamado.Id,
                        NomeArquivo = anexoFile.FileName,
                        CaminhoArquivo = caminhoParaSalvar, // Salva o caminho RELATIVO
                        TipoArquivo = anexoFile.ContentType,
                        TamanhoBytes = anexoFile.Length,
                        DataUpload = DateTime.UtcNow
                    };
                    _contexto.Anexos.Add(novoAnexo);
                }
                await _contexto.SaveChangesAsync(); // Salva os registros dos anexos
            }
            // --- FIM DA CORREÇÃO ---

            // --- LÓGICA DE EMAIL ---
            var prioridadeChamado = await _contexto.Prioridades.FindAsync(novoChamado.IdPrioridade);
            if (prioridadeChamado != null && (prioridadeChamado.Nome == "Médio" || prioridadeChamado.Nome == "Alto" || prioridadeChamado.Nome == "Crítico"))
            {
                try
                {
                    novoChamado.Prioridade = prioridadeChamado;
                    novoChamado.Usuario = usuarioLogado;
                    await _servicoEmail.EnviarEmailChamadoAsync(novoChamado, "novo", _contexto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Falha ao enviar e-mail de novo chamado: {ex.Message}");
                }
            }
            // --- FIM DA LÓGICA DE EMAIL ---

            return CreatedAtAction(
                nameof(ObterChamadoPorId),
                new { id = novoChamado.Id },
                new { id = novoChamado.Id, titulo = novoChamado.Titulo }
            );
        }

        // POST: api/chamados/{id}/mensagem
        [HttpPost("{id}/mensagem")]
        public async Task<IActionResult> AdicionarMensagem(int id, [FromBody] MensagemCriacaoModel model)
        {
            var idUsuarioLogadoClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idUsuarioLogadoClaim, out var idUsuarioLogado)) return Unauthorized();

            var chamado = await _contexto.Chamados.FindAsync(id);
            if (chamado == null) return NotFound("Chamado não encontrado.");

            var tipoAcesso = User.FindFirstValue(ClaimTypes.Role);
            if (tipoAcesso == "Colaborador" && chamado.IdUsuario != idUsuarioLogado)
            {
                return Forbid("Acesso negado.");
            }

            var novaMensagem = new ChamadoMensagem
            {
                IdChamado = id,
                IdUsuarioAutor = idUsuarioLogado,
                Conteudo = model.Conteudo,
                DataEnvio = DateTime.UtcNow
            };
            
            _contexto.ChamadoMensagens.Add(novaMensagem);
            await _contexto.SaveChangesAsync();
            
            var autor = await _contexto.Usuarios.FindAsync(novaMensagem.IdUsuarioAutor);
            
            var viewModel = new MensagemVisaoModel
            {
                Id = novaMensagem.Id,
                Conteudo = novaMensagem.Conteudo,
                DataEnvio = novaMensagem.DataEnvio,
                Autor = autor != null ? new UsuarioVisaoModel { Id = autor.Id, Nome = autor.Nome } : null
            };
            return Ok(viewModel);
        }
        
        // PUT: api/chamados/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarChamado(int id, [FromBody] ChamadoAtualizacaoModel model)
        {
            var idUsuarioLogadoClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idUsuarioLogadoClaim, out var idUsuarioLogado)) return Unauthorized();

            var usuarioLogado = await _contexto.Usuarios.FindAsync(idUsuarioLogado);
            if (usuarioLogado == null) return Unauthorized("Usuário não encontrado.");
            
            var chamado = await _contexto.Chamados.FindAsync(id);
            if (chamado == null) return NotFound("Chamado não encontrado.");

            if (usuarioLogado.ObterTipoAcesso() == "Colaborador" && chamado.IdUsuario != idUsuarioLogado)
            {
                return Forbid("Acesso negado. Você só pode editar seus próprios chamados.");
            }

            chamado.Titulo = model.Titulo;
            chamado.Descricao = model.Descricao;
            chamado.IdTipo = model.IdTipo;
            chamado.IdCategoria = model.IdCategoria;
            
            usuarioLogado.AplicarAtualizacao(chamado, model);

            _contexto.Entry(chamado).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_contexto.Chamados.Any(e => e.Id == id)) { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        // DELETE: api/chamados/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirChamado(int id)
        {
            var idUsuarioLogadoClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tipoAcesso = User.FindFirstValue(ClaimTypes.Role);
            if (!int.TryParse(idUsuarioLogadoClaim, out var idUsuarioLogado)) return Unauthorized();

            var chamado = await _contexto.Chamados.FindAsync(id);
            if (chamado == null) return NotFound("Chamado não encontrado.");

            if (tipoAcesso == "Tecnico")
            {
                return Forbid("Técnicos não podem excluir chamados.");
            }
            
            if (tipoAcesso == "Colaborador" && chamado.IdUsuario != idUsuarioLogado)
            {
                return Forbid("Acesso negado. Você só pode excluir seus próprios chamados.");
            }
            
            // Limpa dependências
            var mensagens = _contexto.ChamadoMensagens.Where(m => m.IdChamado == id);
            _contexto.ChamadoMensagens.RemoveRange(mensagens);

            var anexos = _contexto.Anexos.Where(a => a.IdChamado == id);
            // ANTES DE EXCLUIR O REGISTRO, EXCLUIR O ARQUIVO FÍSICO
            foreach (var anexo in anexos)
            {
                try
                {
                    var physicalPath = Path.Combine(_webHostEnvironment.WebRootPath, anexo.CaminhoArquivo);
                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                }
                catch(Exception ex)
                {
                    // Loga o erro, mas não impede a exclusão do chamado
                    Console.WriteLine($"Erro ao excluir anexo físico: {ex.Message}");
                }
            }
            _contexto.Anexos.RemoveRange(anexos);

            var tecnicos = _contexto.ChamadoTecnicos.Where(t => t.IdChamado == id);
            _contexto.ChamadoTecnicos.RemoveRange(tecnicos);

            // Exclui o chamado
            _contexto.Chamados.Remove(chamado);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}