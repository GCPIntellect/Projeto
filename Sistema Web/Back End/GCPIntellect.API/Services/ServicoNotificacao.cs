using GCPIntellect.API.Data;
using GCPIntellect.API.Models;
using GCPIntellect.API.Services.Interfaces; // Importa as interfaces
using Microsoft.EntityFrameworkCore;

namespace GCPIntellect.API.Services
{
    // CORREÇÃO: Implementa a interface com "I"
    public class ServicoNotificacao : IServicoNotificacao
    {
        private readonly ContextoBD _contexto;
        private readonly IServicoEmail _servicoEmail; // CORREÇÃO: Depende da interface com "I"

        public ServicoNotificacao(ContextoBD contexto, IServicoEmail servicoEmail)
        {
            _contexto = contexto;
            _servicoEmail = servicoEmail;
        }

        public async Task NotificarUsuarioAsync(int idUsuario, string mensagem, string tipo = "info")
        {
            var usuario = await _contexto.Usuarios.FindAsync(idUsuario);
            if (usuario == null || string.IsNullOrEmpty(usuario.Email)) return;

            var notificacao = new NotificacaoFila
            {
                Destinatario = usuario.Email,
                Assunto = "Nova Notificação - GCPIntellect",
                Conteudo = mensagem,
                TipoNotificacao = Models.Enums.TipoNotificacao.EMAIL,
                DataCriacao = DateTime.UtcNow,
                Status = Models.Enums.StatusNotificacao.Pendente
            };

            _contexto.NotificacoesFila.Add(notificacao);
            await _contexto.SaveChangesAsync();
            // (A lógica de realmente ENVIAR o email da fila seria um outro serviço/job)
        }

        public async Task NotificarSobreChamadoAsync(Chamado chamado, string acao)
        {
            // 1. Notifica o usuário que abriu o chamado
            var mensagem = $"Seu chamado #{chamado.Id} ('{chamado.Titulo}') foi {acao}.";
            await NotificarUsuarioAsync(chamado.IdUsuario, mensagem);

            // 2. Notifica os técnicos atribuídos
            var tecnicosIds = await _contexto.ChamadoTecnicos
                .Where(ct => ct.IdChamado == chamado.Id)
                .Select(ct => ct.IdTecnico)
                .ToListAsync();

            foreach (var idTecnico in tecnicosIds)
            {
                if(idTecnico == chamado.IdUsuario) continue; // Não notifica o usuário duas vezes
                
                var mensagemTecnico = $"O chamado #{chamado.Id} ('{chamado.Titulo}'), atribuído a você, foi {acao}.";
                await NotificarUsuarioAsync(idTecnico, mensagemTecnico);
            }
        }

        public async Task NotificarGrupoAsync(string grupo, string mensagem, string tipo = "info")
        {
            // Grupo pode ser "Administrador", "Tecnico" ou "Colaborador"
            var usuarios = await _contexto.Usuarios
                .Where(u => EF.Property<string>(u, "Discriminator") == grupo && u.Ativo)
                .ToListAsync();

            foreach (var usuario in usuarios)
            {
                await NotificarUsuarioAsync(usuario.Id, mensagem, tipo);
            }
        }

        public async Task MarcarComoLidaAsync(int idNotificacao, int idUsuario)
        {
            var usuario = await _contexto.Usuarios.FindAsync(idUsuario);
            if (usuario == null) return;

            var notificacao = await _contexto.NotificacoesFila
                .FirstOrDefaultAsync(n => n.Id == idNotificacao && n.Destinatario == usuario.Email);

            if (notificacao != null)
            {
                notificacao.Status = Models.Enums.StatusNotificacao.Enviado; // "Enviado" aqui significa "Lido"
                notificacao.DataEnvio = DateTime.UtcNow;
                await _contexto.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<NotificacaoFila>> ObterNotificacoesNaoLidasAsync(int idUsuario)
        {
            var usuario = await _contexto.Usuarios.FindAsync(idUsuario);
            if (usuario == null) return Enumerable.Empty<NotificacaoFila>();

            return await _contexto.NotificacoesFila
                .Where(n => n.Destinatario == usuario.Email && n.Status == Models.Enums.StatusNotificacao.Pendente)
                .OrderByDescending(n => n.DataCriacao)
                .ToListAsync();
        }
    }
}