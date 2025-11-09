using GCPIntellect.API.Models;

namespace GCPIntellect.API.Services.Interfaces
{
    public interface IServicoNotificacao
    {
        Task NotificarUsuarioAsync(int idUsuario, string mensagem, string tipo = "info");
        Task NotificarSobreChamadoAsync(Chamado chamado, string acao);
        Task NotificarGrupoAsync(string grupo, string mensagem, string tipo = "info");
        Task MarcarComoLidaAsync(int idNotificacao, int idUsuario);
        Task<IEnumerable<NotificacaoFila>> ObterNotificacoesNaoLidasAsync(int idUsuario);
    }
}