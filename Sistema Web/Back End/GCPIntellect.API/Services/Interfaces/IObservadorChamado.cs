using GCPIntellect.API.Models;

namespace GCPIntellect.API.Services.Interfaces
{
    public interface IObservadorChamado
    {
        Task OnChamadoAlteradoAsync(Chamado chamado, string acao);
        Task OnNovaMensagemAsync(ChamadoMensagem mensagem);
        Task OnTecnicoAtribuidoAsync(int idChamado, int idTecnico);
    }
}